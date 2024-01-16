﻿using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
 
namespace AdvansysRevitAssembly.Logic.ElementsViewsHelper
{
    /// <summary>
    /// Revit Document and IEnumerable<Element>
    /// extension methods.
    /// </summary>
    static class ElementViewsHelper
    {
        /// <summary>
        /// Return an enumeration of all views in this
        /// document that can display elements at all.
        /// </summary>
        static IEnumerable<View>FindAllViewsThatCanDisplayElements(this Document doc)
        {
            ElementMulticlassFilter filter
              = new ElementMulticlassFilter(
                new List<Type> {
            typeof( View3D ),
            typeof( ViewPlan ),
            typeof( ViewSection ) });

            return new FilteredElementCollector(doc)
              .WherePasses(filter)
              .Cast<View>()
              .Where(v => !v.IsTemplate);
        }

        /// <summary>
        /// Return all views that display 
        /// any of the given elements.
        /// </summary>
        public static IEnumerable<View>FindAllViewsWhereAllElementsVisible(this IEnumerable<Element> elements)
        {
            if (null == elements)
            {
                throw new ArgumentNullException("elements");
            }

            //if( 0 == elements.Count )
            //{
            //  return new List<View>();
            //}

            Element e1 = elements.FirstOrDefault<Element>();

            if (null == e1)
            {
                return new List<View>();
            }

            Document doc = e1.Document;

            IEnumerable<View> relevantViewList
              = doc.FindAllViewsThatCanDisplayElements();

            IEnumerable<ElementId> idsToCheck
              = (from e in elements select e.Id);

            return (
              from v in relevantViewList
              let idList
          = new FilteredElementCollector(doc, v.Id)
            .WhereElementIsNotElementType()
            .ToElementIds()
              where !idsToCheck.Except(idList).Any()
              select v);
        }
    }
}