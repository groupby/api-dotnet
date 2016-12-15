using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using GroupByInc.Api.Exceptions;
using GroupByInc.Api.Models;
using GroupByInc.Api.Url;
using GroupByInc.Api.Util;

namespace GroupByInc.Api.Tags
{
    public class UrlFunctions
    {
        private static Mappers _mappers;

        static UrlFunctions()
        {
            _mappers = new Mappers();
        }

        public static string ToUrlAdd(string identifier, string searchString, List<Navigation> navigations,
            string navigationName, Refinement refinement)
        {
            UrlBeautifier urlBeautifier = GetBeautifier(identifier);
            Query refinements = AddRefinements(navigations, navigationName, refinement);
            try
            {
                return urlBeautifier.ToUrl(searchString, refinements.GetRefinementsString());
            }
            catch (UrlBeautificationException e)
            {
                throw new Exception("Unable to add url", e);
            }
        }

        public static string ToUrlRemove(string identifier, string searchString, List<Navigation> navigations,
            string navigationName, Refinement refinement)
        {
            UrlBeautifier urlBeautifier = GetBeautifier(identifier);
            Query refinements = RemoveRefinements(navigations, navigationName, refinement);
            try
            {
                return urlBeautifier.ToUrl(searchString, refinements.GetRefinementsString());
            }
            catch (UrlBeautificationException e)
            {
                throw new Exception("Unable to add url", e);
            }
        }

        private static Query RemoveRefinements(List<Navigation> navigations, string navigationName,
            Refinement refinement)
        {
            Query query = new Query();
            OrderedDictionary queryNavigations = query.GetNavigations();
            foreach (Navigation navigation in navigations)
            {
                queryNavigations.Add(navigation.GetName(), navigation);
            }

            string stringRefinements = query.GetRefinementsString();
            query = new Query();
            query.AddRefinementsByString(stringRefinements);
            queryNavigations = query.GetNavigations();
            if (queryNavigations == null)
            {
                throw new Exception("No existing refinements so cannot remove a refinement");
            }
            if (refinement != null)
            {
                IDictionaryEnumerator dictionaryEnumerator = queryNavigations.GetEnumerator();
                List<Navigation> deleteNavigations = new List<Navigation>();
                while (dictionaryEnumerator.MoveNext())
                {
                    Navigation n = (Navigation) dictionaryEnumerator.Value;
                    if (n.GetName().Equals(navigationName))
                    {
                        List<Refinement>.Enumerator enumerator = n.GetRefinements().GetEnumerator();
                        List<Refinement> deletedRefinements = new List<Refinement>();
                        while (enumerator.MoveNext())
                        {
                            Refinement r = enumerator.Current;
                            if (r != null && r.ToTildeString().Equals(refinement.ToTildeString()))
                            {
                                deletedRefinements.Add(r);
                            }
                        }

                        foreach (Refinement deletedRefinement in deletedRefinements)
                        {
                            n.GetRefinements().Remove(deletedRefinement);
                        }

                        if (n.GetRefinements().Count == 0)
                        {
                            deleteNavigations.Add(n);
                        }
                    }
                }

                foreach (Navigation deletedNavigation in deleteNavigations)
                {
                    queryNavigations.Remove(deletedNavigation);
                }
            }
            return query;
        }

        private static Query AddRefinements(List<Navigation> navigations, string navigationName, Refinement refinement)
        {
            Query query = new Query();
            OrderedDictionary queryNavigations = query.GetNavigations();
            if (navigations != null)
            {
                List<Navigation> navs = _mappers.CloneJson(navigations);
                foreach (Navigation navigation in navs)
                {
                    queryNavigations.Add(navigation.GetName(), navigation);
                }
            }
            if (queryNavigations[navigationName] == null)
            {
                queryNavigations.Add(navigationName, new Navigation().SetName(navigationName));
            }
            if (refinement != null)
            {
                ((Navigation) queryNavigations[navigationName]).GetRefinements().Add(refinement);
            }
            return query;
        }

        private static UrlBeautifier GetBeautifier(string identifier)
        {
            UrlBeautifier urlBeautifier = UrlBeautifier.GetUrlBeautifiers()[identifier];
            if (urlBeautifier == null)
            {
                throw new Exception(
                    string.Format(
                        "Could not find UrlBeautifier named: {0}. Please call UrlBeautifier.createUrlBeautifier(String) to instantiate",
                        identifier));
            }
            return urlBeautifier;
        }
    }
}