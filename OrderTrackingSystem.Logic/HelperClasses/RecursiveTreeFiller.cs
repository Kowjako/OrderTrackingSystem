using System.Collections.Generic;
using System.Linq;

namespace OrderTrackingSystem.Logic.HelperClasses
{
    public interface IComposite<T>
    {
        int Id { get; set; }
        string Name { get; set; }
        List<T> Children { get; set; }
        int? ParentId { get; set; }
    }

    public class RecursiveTreeFiller<T> where T : IComposite<T>
    {
        public static void FillTreeRecursive(List<T> allObjects)
        {
            /* Wybieramy wszystkich rodzicow, parentId = null i wypelniamy je */
            var parentFolders = allObjects.Where(p => p.ParentId == null);
            foreach (var parentFolder in parentFolders)
            {
                FillParent(parentFolder, allObjects);
            }
        }

        private static void FillParent(T parentObject, List<T> allChildrensToSeparate)
        {
            parentObject.Children.AddRange(allChildrensToSeparate.Where(f => f.ParentId == parentObject.Id));
            if (parentObject.Children.Any())
            {
                foreach (var subFolder in parentObject.Children)
                {
                    FillParent(subFolder, allChildrensToSeparate);
                }
            }
            else
            {
                /* Warunek wyjscia z rekurencji */
                return;
            }
        }

        public static List<T> GetAllChild (T source)
        {
            var list = new List<T>();
            foreach (var child in source.Children)
            {
                list.Add(child);
                /* rekurencyjnie dodajemy */
                list.AddRange(GetAllChild(child));
            }
            return list;

        }
    }
}
