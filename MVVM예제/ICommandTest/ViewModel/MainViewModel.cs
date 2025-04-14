using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ICommandTest.Model;
using ICommandTest.ViewModel.Commands;

namespace ICommandTest.ViewModel
{
    public class MainViewModel
    {
        // View의 코드 비하인드에서
        // 버튼 클릭 이벤트 핸들러에 작성한 로직을
        // ViewModel에서 함수로 구현합니다.
        public User user { get; set; } = new User() 
        {
            Id = "ID",
            Pw = "PW"
        };
        public Command Cmd { get; set; }
        public MainViewModel() 
        {
            Cmd = new Command(DisplayMessage);
        }
        public void DisplayMessage()
        {
            MessageBox.Show($"아이디 : {user.Id}, 비밀번호 : {user.Pw}");
        }
    }
}
