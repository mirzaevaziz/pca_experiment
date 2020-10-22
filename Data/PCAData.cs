using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using FinderOfStandarts.Models;

namespace FinderOfStandarts.Data
{
    class PCAData
    {
        public ObjectSet GetObjectSet(int featureCount, decimal classValue)
        {
            var features = new List<Feature>();
            for (int i = 0; i < featureCount + 1; i++)
            {
                features.Add(new Feature()
                {
                    // Index = i++,
                    IsActive = true,
                    IsContinuous = i != featureCount,
                    IsClass = i == featureCount,
                    Name = $"Ft {i:000}"
                });
            }

            var objects = new List<ObjectData>();
            int ind = 0;
            using (var reader = new StreamReader(Path.Combine("Data", "PcaFiles", $"{featureCount}.txt")))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    objects.Add(new ObjectData()
                    {
                        Data = Array.ConvertAll(line.Split(' '), x => decimal.Parse(x, NumberStyles.Any, CultureInfo.InvariantCulture)),
                        Index = ind++
                    });
                }
            }

            var set = new ObjectSet(objects, features, classValue, $"{featureCount}.txt");

            return set;
        }
    }
}