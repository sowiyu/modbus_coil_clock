using System;
using System.Drawing;
using System.Net.Sockets;
using System.Windows.Forms;
using Modbus.Device;

namespace modbus_coil_test
{
    public partial class Form1 : Form
    {
        private TcpClient _tcpClient;
        private ModbusIpMaster _master;

        private System.Windows.Forms.Timer _monitorTimer;    // 모니터링용
        private System.Windows.Forms.Timer _autoActionTimer; // 961번 깜빡임용

        private const byte SlaveId = 1; // 국번

        public Form1()
        {
            InitializeComponent();
            InitializeCustomTimers();
        }

        private void InitializeCustomTimers()
        {
            // 1. 모니터링 (0.5초마다 6, 7번 확인)
            _monitorTimer = new System.Windows.Forms.Timer();
            _monitorTimer.Interval = 500;
            _monitorTimer.Tick += MonitorTimer_Tick;

            // 2. 자동 동작 (961번 깜빡임용)
            _autoActionTimer = new System.Windows.Forms.Timer();
            _autoActionTimer.Tick += AutoActionTimer_Tick;
        }

        // [연결 버튼]
        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (_tcpClient == null || !_tcpClient.Connected)
                {
                    _tcpClient = new TcpClient(txtIpAddress.Text, 502);
                    _master = ModbusIpMaster.CreateIp(_tcpClient);

                    _monitorTimer.Start();
                    btnConnect.Text = "연결 해제";
                    btnConnect.BackColor = Color.LightGreen;
                    MessageBox.Show("연결 성공!");
                }
                else
                {
                    Disconnect();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("연결 실패: " + ex.Message);
                Disconnect();
            }
        }

        private void Disconnect()
        {
            _monitorTimer.Stop();
            _autoActionTimer.Stop();
            btnStartAuto.Text = "타이머 시작";

            if (_tcpClient != null) { _tcpClient.Close(); _tcpClient = null; }

            btnConnect.Text = "연결";
            btnConnect.BackColor = Color.LightGray;

            lblSensor6.BackColor = Color.Silver;
            lblSensor7.BackColor = Color.Silver;
        }

        // [모니터링] 6번, 7번 상태 읽기
        private void MonitorTimer_Tick(object sender, EventArgs e)
        {
            if (_master == null) return;
            try
            {
                bool[] inputs = _master.ReadCoils(SlaveId, 6, 2);

                // 6번 상태
                if (inputs[0])
                {
                    lblSensor6.BackColor = Color.Red;
                    lblSensor6.Text = "ON (6)";
                }
                else
                {
                    lblSensor6.BackColor = Color.Gray;
                    lblSensor6.Text = "OFF (6)";
                }

                // 7번 상태
                if (inputs[1])
                {
                    lblSensor7.BackColor = Color.Red;
                    lblSensor7.Text = "ON (7)";
                }
                else
                {
                    lblSensor7.BackColor = Color.Gray;
                    lblSensor7.Text = "OFF (7)";
                }
            }
            catch { }
        }

        // [버튼 960] 수동 토글 (기존 이름 유지: _1)
        private void btnManual960_Click_1(object sender, EventArgs e)
        {
            if (_master == null) return;
            try
            {
                // 960 읽어서 반대로 쓰기
                bool[] status = _master.ReadCoils(SlaveId, 960, 1);
                _master.WriteSingleCoil(SlaveId, 960, !status[0]);
            }
            catch (Exception ex)
            {
                MessageBox.Show("960 오류: " + ex.Message);
            }
        }

        // [버튼 961] 수동 토글 (새로 연결할 함수)
        private void btnManual961_Click(object sender, EventArgs e)
        {
            if (_master == null) return;
            try
            {
                // 961 읽어서 반대로 쓰기
                bool[] status = _master.ReadCoils(SlaveId, 961, 1);
                _master.WriteSingleCoil(SlaveId, 961, !status[0]);
            }
            catch (Exception ex)
            {
                MessageBox.Show("961 오류: " + ex.Message);
            }
        }

        // [타이머 버튼] 961번 자동 깜빡임 시작/정지
        private void btnStartAuto_Click(object sender, EventArgs e)
        {
            if (_autoActionTimer.Enabled)
            {
                _autoActionTimer.Stop();
                btnStartAuto.Text = "타이머 시작";
            }
            else
            {
                if (int.TryParse(txtInterval.Text, out int intervalMs))
                {
                    if (intervalMs < 50) intervalMs = 50; // 최소 속도 제한

                    _autoActionTimer.Interval = intervalMs;
                    _autoActionTimer.Start();
                    btnStartAuto.Text = "타이머 중지";
                }
                else
                {
                    MessageBox.Show("시간(ms)을 숫자로 입력하세요.");
                }
            }
        }

        // [타이머 동작] 여기가 중요! 961번을 깜빡이게 함
        private void AutoActionTimer_Tick(object sender, EventArgs e)
        {
            if (_master == null) return;
            try
            {
                // 961번을 읽어서 -> 반대로 바꿈 (깜빡임 효과)
                bool[] status = _master.ReadCoils(SlaveId, 961, 1);
                _master.WriteSingleCoil(SlaveId, 961, !status[0]);
            }
            catch
            {
                // 통신 오류 시 무시
            }
        }

        // 오류 방지용 빈 함수들
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void button1_Click(object sender, EventArgs e) { btnConnect_Click(sender, e); }
    }
}