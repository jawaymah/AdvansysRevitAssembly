using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using AdvansysRevitAssembly.Logic;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace AdvansysRevitAssembly.Commands
{

    [Transaction(TransactionMode.Manual)]
    public class AutomaticBOMCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;
            var selection = commandData.Application.ActiveUIDocument.Selection.GetElementIds().ToList();
            Result res = CreateSchedule(commandData, ref message);
            return BomPdfCreator.CreateReport(ref message, doc, selection);
        }
        public Result CreateSchedule(ExternalCommandData commandData, ref string message)
        {
            // Get the current document
            Document doc = commandData.Application.ActiveUIDocument.Document;

            // Start a transaction
            using (Transaction tx = new Transaction(doc))
            {
                tx.Start("Create Schedule");

                // Create a new schedule for a specific category, e.g., Walls
                ViewSchedule schedule = ViewSchedule.CreateSchedule(doc, new ElementId(BuiltInCategory.OST_MechanicalEquipment));

                // Find the ElementId of the project parameters
                ElementId firstParamId_ = null;// FindParameterId(doc, "Family");
                ElementId secondParamId_ = null;// FindParameterId(doc, "Type");
                ElementId thirdParamId_ = null;// FindParameterId(doc, "Description");
                var fields  = schedule.Definition.GetSchedulableFields();
                foreach (SchedulableField field in fields)
                {
                    var name = field.GetName(doc);
                    if (name == "Family")
                    {
                        firstParamId_ = field.ParameterId;
                    }
                    else if (name == "Type")
                    {
                        secondParamId_ = field.ParameterId;
                    }
                    else if (name == "Description")
                    {
                        thirdParamId_ = field.ParameterId;
                    }
                }
                // Add Family field to the schedule
                ScheduleField familyField = schedule.Definition.AddField(ScheduleFieldType.Instance, firstParamId_);

                // Add Type field to the schedule
                ScheduleField typeField = schedule.Definition.AddField(ScheduleFieldType.Instance, secondParamId_);

                // Add Description field to the schedule
                // Assuming 'Description' is a valid parameter for the elements
                ScheduleField descriptionField = schedule.Definition.AddField(ScheduleFieldType.ElementType, thirdParamId_);

                // Group the schedule by 'Family'
                ScheduleSortGroupField groupFieldFamily = new ScheduleSortGroupField(familyField.FieldId);
                groupFieldFamily.ShowHeader = true;
                schedule.Definition.AddSortGroupField(groupFieldFamily);

                // Then group by 'Type'
                ScheduleSortGroupField groupFieldType = new ScheduleSortGroupField(typeField.FieldId);
                schedule.Definition.AddSortGroupField(groupFieldType);

                schedule.Definition.ShowGrandTotal = true;
                schedule.Definition.ShowGrandTotalCount = true;
                // Optionally, format the schedule

                tx.Commit();
                commandData.Application.ActiveUIDocument.ActiveView = schedule;
                return Result.Succeeded;
            }
            return Result.Succeeded;
            // Define the project parameter names
            string firstParameterName = "FirstParameter"; // Replace with your first parameter name
            string secondParameterName = "SecondParameter"; // Replace with your second parameter name

            // Find the ElementId of the project parameters
            ElementId firstParamId = FindParameterId(doc, firstParameterName);
            ElementId secondParamId = FindParameterId(doc, secondParameterName);

            if (firstParamId == null || secondParamId == null)
            {
                message = "Parameter not found.";
                return Result.Failed;
            }

            // Start a transaction
            using (Transaction tx = new Transaction(doc))
            {
                tx.Start("Create Schedule");

                // Create a new schedule for a specific category
                ViewSchedule schedule = ViewSchedule.CreateSchedule(doc, new ElementId(BuiltInCategory.OST_MechanicalEquipment)); // Replace OST_Walls with your category

                // Add fields to the schedule
                ScheduleField field1 = schedule.Definition.AddField(ScheduleFieldType.Instance, firstParamId);
                ScheduleField field2 = schedule.Definition.AddField(ScheduleFieldType.Instance, secondParamId);

                // Group the schedule by the first parameter
                schedule.Definition.IsItemized = false; // Turn off itemization to enable grouping
                ScheduleSortGroupField sortGroupField = new ScheduleSortGroupField(field1.FieldId);
                schedule.Definition.AddSortGroupField(sortGroupField);

                // Optionally, format the schedule

                tx.Commit();
                return Result.Succeeded;
            }
        }

        private ElementId FindParameterId(Document doc, string parameterName)
        {
            // Search for a project parameter with the given name and return its Id
            foreach (ParameterElement pe in new FilteredElementCollector(doc).OfClass(typeof(ParameterElement)))
            {
                if (pe.Name.Equals(parameterName))
                {
                    return pe.Id;
                }
            }
            return null;
        }
    }
}
