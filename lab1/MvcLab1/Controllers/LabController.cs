using Microsoft.AspNetCore.Mvc;

namespace MvcLab.Controllers
{
    public class LabController : Controller
    {
        public IActionResult Info()
        {
            var model = new LabInfo
            {
                Number = 2,
                Topic = "������ ASP.NET MVC",
                Goal = "��������� ���������� ����������, ���������� ��� � View �� ���������� ��",
                StudentName = "����",
                StudentSurname = "�������"
            };

            return View(model);
        }
    }

    public class LabInfo
    {
        public int Number { get; set; }
        public string Topic { get; set; }
        public string Goal { get; set; }
        public string StudentName { get; set; }
        public string StudentSurname { get; set; }
    }
}
