using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace school_project.Models
{
    public interface IProjectRepository
    {
        SingleImageDiagnosis GetProject(int Id);
        IEnumerable<SingleImageDiagnosis> GetAllSingleImageDiagnosisProjects();
        SingleImageDiagnosis Add(SingleImageDiagnosis singleImageDiagnosis);
        SingleImageDiagnosis Update(SingleImageDiagnosis singleImageDiagnosisChanges);

        SingleImageDiagnosis Delete(int id);
    }
}
