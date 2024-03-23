namespace Website_Mvc.Models
{
	public class DoctorProfileViewModel
	{
		public (int IdUser, string FullName, string Address, string DepartmentName, string DiseaseName, string UserImage, string DepartmentImage, int ReviewCount) Doctor { get; set; }
		public IEnumerable<string> DoctorDiseases { get; set; }
		public IEnumerable<(PatientReviewsDoctor review, string reviewerName)> GetDoctorReviews { get; set; } // Thêm danh sách đánh giá của bác sĩ

	}
}