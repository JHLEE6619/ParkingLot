using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client1.Model;
using Client1.ViewModel.Commands;

namespace Client1.ViewModel
{
    public class VM_Main
    {
        // 클라이언트 실행 시 서버는 연결과 함께 주차 차량 리스트를 클라이언트로 보낸다.
        // 클라이언트는 입/출차 카메라로 찍은 이미지를 3초마다 서버로 보낸다.
        // 서버는 입/출차 시 차량 정보를 메세지에 실어 보낸다.
        // 클라이언트는 항상 read 대기상태 -> 스레드 생성

        // 시작 시 -> 주차 차량 리스트 요청
        public ObservableCollection<Record> Record { get; set; } = [];
        public ObservableCollection<Brush> Color { get; set; } = [];
        public VM_Main() 
        {
            Init_color();
        }

        private void Init_color()
        {
            const int seat_cnt = 21;
            for (int i = 0; i < seat_cnt; i++)
            {
                Color.Add(Brushes.Aqua);
            }
        }
    }
}
