using System.Collections.Generic;
using System.IO;
using System.Linq;
using FinderOfStandarts.Models;

namespace FinderOfStandarts.Methods
{
    class StandartObjectsFinder
    {
        public static HashSet<int> Find(ObjectSet set,
                                           IEnumerable<HashSet<int>> groups,
                                           IEnumerable<Sphere> spheres,
                                           HashSet<int> excludedObjects,
                                           decimal[,] distances)
        {
            var objects = groups.SelectMany(s => s).ToHashSet<int>();
            System.Console.WriteLine("Finding standart objects...");
            foreach (var group in groups.OrderByDescending(o => o.Count))
            {
                var candidates = spheres.Where(w => group.Contains(w.ObjectIndex.Value))
                                        .OrderBy(o => o.Radius)
                                        .Select(s => s.ObjectIndex.Value);


                foreach (var candidate in candidates)
                {

                    excludedObjects.Add(candidate);
                    foreach (var obj in objects)
                    {
                        var ss = objects.Except(excludedObjects).Select(s => new
                        {
                            ObjectIndex = s,
                            Distance = distances[obj, s] / spheres.First(f => f.ObjectIndex == s).Radius,
                            Class = set.Objects[s][set.ClassFeatureIndex]
                        }).OrderBy(o => o.Distance);

                        var minDistance = ss.Min(m => m.Distance);

                        bool isWrongRecognition = ss.Where(w => w.Distance == minDistance).Any(a => set.Objects[a.ObjectIndex][set.ClassFeatureIndex] != set.Objects[obj][set.ClassFeatureIndex]);

                        if (isWrongRecognition)
                        {
                            excludedObjects.Remove(candidate);
                            break;
                        }
                    }
                }
            }
            return objects.Except(excludedObjects).ToHashSet<int>();
        }
    }
}