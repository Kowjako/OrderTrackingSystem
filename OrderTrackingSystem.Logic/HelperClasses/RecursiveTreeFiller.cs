using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public static void FillTreeRecursive(T parentObject, List<T> allChildrensToSeparate)
        {
            parentObject.Children.AddRange(allChildrensToSeparate.Where(f => f.ParentId == parentObject.Id));
            if (parentObject.Children.Any())
            {
                foreach (var subFolder in parentObject.Children)
                {
                    FillTreeRecursive(subFolder, allChildrensToSeparate);
                }
            }
            else
            {
                /* Warunek wyjscia z rekurencji */
                return;
            }
        }
    }
}
