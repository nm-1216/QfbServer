using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace QfbServer.Models
{
    //public class QfbServerContextInitializer : CreateDatabaseIfNotExists<QfbServerContext>
    //public class QfbServerContextInitializer : DropCreateDatabaseIfModelChanges<QfbServerContext>
    public class QfbServerContextInitializer : DropCreateDatabaseAlways<QfbServerContext>
    {
        protected override void Seed(QfbServerContext context)
        {
            var pages = new List<Page>()
            {
                new Page() {
                    PageName = "Page 1",
                    MeasurePoints = new List<string> {
                        "vsd123,CC", "vsd123,CC"
                    },
                    Pictures = new List<string>()
                }
            };

            Project project = new Project()
            {
                ProjectName = "Car",
                ProductName = "Door",
                TargetName = "Hole",
                Pages = pages
            };

            pages.ForEach(page => page.Project = project);

            pages.ForEach(page => context.Pages.Add(page));

            context.Projects.Add(project);

            //MeasureData measureData = new MeasureData();
            //context.MeasureDatas.Add(measureData);

            context.SaveChanges();
        }
    }
}