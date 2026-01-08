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

        // --- 타이머 목록 ---
        private System.Windows.Forms.Timer _monitorTimer;    // 모니터링 + 시간표시
        private System.Windows.Forms.Timer _blinkTimer;      // 단순 깜빡임
        private System.Windows.Forms.Timer _cycleWaitTimer;  // 순환 대기 (분 단위)
        private System.Windows.Forms.Timer _pulseOnTimer;    // 펄스 켜짐 (ms 단위)

        // 시간 체크용 변수
        private DateTime _cycleStartTime;

        private const byte SlaveId = 1;

        public Form1()
        {
            InitializeComponent();
            InitializeTimers();
        }

        private void InitializeTimers()
        {
            // 1. 모니터링 (0.5초)
            _monitorTimer = new System.Windows.Forms.Timer();
            _monitorTimer.Interval = 500;
            _monitorTimer.Tick += MonitorTimer_Tick;

            // 2. 단순 깜빡임
            _blinkTimer = new System.Windows.Forms.Timer();
            _blinkTimer.Tick += BlinkTimer_Tick;

            // 3. 순환 대기 (분 단위)
            _cycleWaitTimer = new System.Windows.Forms.Timer();
            _cycleWaitTimer.Tick += CycleWaitTimer_Tick;

            // 4. 펄스 OFF (ms 단위)
            _pulseOnTimer = new System.Windows.Forms.Timer();
            _pulseOnTimer.Tick += PulseOnTimer_Tick;
        }

        // =================================================================================
        // [1] 연결 / 해제
        // =================================================================================
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
            _blinkTimer.Stop();
            _cycleWaitTimer.Stop();
            _pulseOnTimer.Stop();

            btnStartAuto.Text = "단순 깜빡임 시작";
            btnStartCycle.Text = "순환 시작";
            lblCycleStatus.Text = "대기중";

            if (_tcpClient != null) { _tcpClient.Close(); _tcpClient = null; }

            btnConnect.Text = "연결";
            btnConnect.BackColor = Color.LightGray;
            lblSensor6.BackColor = Color.Silver;
            lblSensor7.BackColor = Color.Silver;

            btnManual960.BackColor = SystemColors.Control;
            btnManual961.BackColor = SystemColors.Control;
        }

        // =================================================================================
        // [2] 모니터링 (6, 7번 읽기 + 시간 표시)
        // =================================================================================
        private void MonitorTimer_Tick(object sender, EventArgs e)
        {
            // 순환 타이머가 돌고 있을 때만 시간 표시
            if (_cycleWaitTimer.Enabled)
            {
                TimeSpan elapsed = DateTime.Now - _cycleStartTime;
                lblCycleStatus.Text = $"{(int)elapsed.TotalMinutes}분 {elapsed.Seconds}초 경과";
            }
            // 펄스가 켜져있는 잠깐 동안은 "작동중" 표시 (타이머가 멈춰있으므로)
            else if (_pulseOnTimer.Enabled)
            {
                lblCycleStatus.Text = "!!! 작동중 (ON) !!!";
            }

            if (_master == null) return;
            try
            {
                bool[] inputs = _master.ReadCoils(SlaveId, 6, 2);

                if (inputs[0]) { lblSensor6.BackColor = Color.Red; lblSensor6.Text = "ON (6)"; }
                else { lblSensor6.BackColor = Color.Gray; lblSensor6.Text = "OFF (6)"; }

                if (inputs[1]) { lblSensor7.BackColor = Color.Red; lblSensor7.Text = "ON (7)"; }
                else { lblSensor7.BackColor = Color.Gray; lblSensor7.Text = "OFF (7)"; }
            }
            catch { }
        }

        // =================================================================================
        // [3] 수동 버튼 (960, 961)
        // =================================================================================
        private void btnManual960_Click_1(object sender, EventArgs e)
        {
            if (_master == null) return;
            try
            {
                bool[] status = _master.ReadCoils(SlaveId, 960, 1);
                bool newState = !status[0];
                _master.WriteSingleCoil(SlaveId, 960, newState);

                if (newState) btnManual960.BackColor = Color.LightGreen;
                else btnManual960.BackColor = Color.IndianRed;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void btnManual961_Click(object sender, EventArgs e)
        {
            if (_master == null) return;
            try
            {
                bool[] status = _master.ReadCoils(SlaveId, 961, 1);
                bool newState = !status[0];
                _master.WriteSingleCoil(SlaveId, 961, newState);
                Update961ButtonColor(newState);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void Update961ButtonColor(bool isOn)
        {
            if (isOn) btnManual961.BackColor = Color.LightGreen;
            else btnManual961.BackColor = Color.IndianRed;
        }

        // =================================================================================
        // [4] 단순 깜빡임
        // =================================================================================
        private void btnStartAuto_Click(object sender, EventArgs e)
        {
            _cycleWaitTimer.Stop();
            _pulseOnTimer.Stop(); // 혹시 켜져있던 것도 끔
            btnStartCycle.Text = "순환 시작";
            lblCycleStatus.Text = "대기중";

            if (_blinkTimer.Enabled)
            {
                _blinkTimer.Stop();
                btnStartAuto.Text = "단순 깜빡임 시작";
            }
            else
            {
                if (int.TryParse(txtInterval.Text, out int intervalMs) && intervalMs >= 50)
                {
                    _blinkTimer.Interval = intervalMs;
                    _blinkTimer.Start();
                    btnStartAuto.Text = "단순 깜빡임 중지";
                }
                else MessageBox.Show("ms 시간을 확인하세요");
            }
        }

        private void BlinkTimer_Tick(object sender, EventArgs e)
        {
            if (_master == null) return;
            try
            {
                bool[] status = _master.ReadCoils(SlaveId, 961, 1);
                bool newState = !status[0];
                _master.WriteSingleCoil(SlaveId, 961, newState);
                Update961ButtonColor(newState);
            }
            catch { }
        }

        // =================================================================================
        // [5] 순환 타이머 (★수정됨: OFF 되고 나서 시간 다시 잼★)
        // =================================================================================
        private void btnStartCycle_Click(object sender, EventArgs e)
        {
            _blinkTimer.Stop();
            btnStartAuto.Text = "단순 깜빡임 시작";

            if (_cycleWaitTimer.Enabled || _pulseOnTimer.Enabled)
            {
                _cycleWaitTimer.Stop();
                _pulseOnTimer.Stop();
                btnStartCycle.Text = "순환 시작";
                lblCycleStatus.Text = "대기중";
            }
            else
            {
                if (int.TryParse(txtCycleMin.Text, out int minutes) &&
                    int.TryParse(txtInterval.Text, out int pulseMs))
                {
                    int waitMs = minutes * 60 * 1000;
                    if (waitMs <= 0 || pulseMs <= 0) { MessageBox.Show("0보다 큰 숫자 입력"); return; }

                    _cycleWaitTimer.Interval = waitMs;

                    // 처음 시작할 때 시간 기록
                    _cycleStartTime = DateTime.Now;
                    _cycleWaitTimer.Start();

                    btnStartCycle.Text = "순환 중지(ON)";
                    lblCycleStatus.Text = "0분 0초 경과";
                }
                else
                {
                    MessageBox.Show("숫자 입력 확인");
                }
            }
        }

        // 대기 시간(예: 60분)이 끝났을 때
        private void CycleWaitTimer_Tick(object sender, EventArgs e)
        {
            // ★ 중요: 대기 타이머를 멈춥니다! (시간 카운트 중단)
            _cycleWaitTimer.Stop();

            if (_master == null) return;
            try
            {
                // 961 ON
                _master.WriteSingleCoil(SlaveId, 961, true);
                Update961ButtonColor(true);

                // 펄스 타이머 시작 (예: 500ms 뒤에 꺼지도록)
                _pulseOnTimer.Interval = int.Parse(txtInterval.Text);
                _pulseOnTimer.Start();
            }
            catch { }
        }

        // 펄스 시간(예: 500ms)이 끝나고 꺼질 때
        private void PulseOnTimer_Tick(object sender, EventArgs e)
        {
            _pulseOnTimer.Stop(); // 펄스 타이머 정지

            if (_master == null) return;
            try
            {
                // 961 OFF
                _master.WriteSingleCoil(SlaveId, 961, false);
                Update961ButtonColor(false);

                // ★ 중요: 꺼지고 나서야 다시 대기 타이머를 시작합니다.
                // 이때 시간을 현재 시간으로 초기화합니다.
                _cycleStartTime = DateTime.Now;
                _cycleWaitTimer.Start();
            }
            catch { }
        }

        // 오류 방지
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void button1_Click(object sender, EventArgs e) { btnConnect_Click(sender, e); }
    }
}