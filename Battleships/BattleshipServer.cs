using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace Battleships
{
    public class Form1 : Form
    {
        private const int MaxMarks = 60;

        private const int MaxAllowedCommands = 20;

        private const int ShiptypeBattleship = 0;

        private const int ShiptypeFrigate = 1;

        private const int ShiptypeSubmarine = 2;

        private const int MaxShips = 200;

        private const int FireLimit = 4;

        private const bool TutorVersion = false;

        private const int PortToClient = 1925;

        private const int PortFromClient = 1924;

        private const int DefaultServer = 0;

        private const int NastyServer = 1;

        private const int ZeroServer = 2;

        private const int MaxNasties = 20;

        private const int ServerType = NastyServer;

        private readonly Random _random = new Random();

        public UdpClient ReceivingUdpClient;

        public IPEndPoint RemoteIpEndPoint;

        public Thread ThreadReceive;

        private readonly int[,] _area;

        private readonly int[,] _distance;

        private bool _displayNames;

        private readonly bool[] _inuse;

        private readonly bool[] _active;

        private readonly string[] _studentId;

        private readonly string[] _studentFirstname;

        private readonly string[] _studentFamilyname;

        private readonly string[] _ipAddress;

        private readonly int[] _shipType;

        private readonly string[] _destinationAddress;

        private readonly string[] _sourceAddress;

        private readonly string[] _message;

        private readonly string[] _displayId;

        private readonly int[] _displayScore;

        private readonly int[] _shipX;

        private readonly int[] _shipY;

        private readonly bool[] _moves;

        private readonly int[] _speedX;

        private readonly int[] _speedY;

        private readonly bool[] _fire;

        private readonly int[] _fireX;

        private readonly int[] _fireY;

        private readonly int[] _fireCount;

        private readonly int[] _health;

        private readonly int[] _newHealth;

        private readonly int[] _flag;

        private readonly int[] _score;

        private readonly int[] _marks;

        private readonly int[] _commandCount;

        private int _numberOfShips;

        private readonly int[] _listOfShips;

        private int _packX;

        private int _packY;

        private int _packTimer;

        private long _tenMinuteCounter;

        private bool _showMarks;

        private IContainer components;

        [AccessedThroughProperty("BattleArea"), CompilerGenerated] private PictureBox _battleArea;

        [AccessedThroughProperty("OneSecondTimer"),
         CompilerGenerated] private Timer _oneSecondTimer;

        [AccessedThroughProperty("btnRestore"), CompilerGenerated] private Button _btnRestore;

        internal virtual PictureBox BattleArea
        {
            [CompilerGenerated] get { return _battleArea; }
            [CompilerGenerated]
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                EventHandler value2 = BattleArea_Click;
                EventHandler value3 = BattleArea_DoubleClick;
                var battleArea = _battleArea;
                if (battleArea != null)
                {
                    battleArea.Click -= value2;
                    battleArea.DoubleClick -= value3;
                }
                _battleArea = value;
                battleArea = _battleArea;
                if (battleArea != null)
                {
                    battleArea.Click += value2;
                    battleArea.DoubleClick += value3;
                }
            }
        }

        internal virtual Timer OneSecondTimer
        {
            [CompilerGenerated] get { return _oneSecondTimer; }
            [CompilerGenerated]
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                EventHandler value2 = OneSecondTimer_Tick;
                var oneSecondTimer = _oneSecondTimer;
                if (oneSecondTimer != null)
                {
                    oneSecondTimer.Tick -= value2;
                }
                _oneSecondTimer = value;
                oneSecondTimer = _oneSecondTimer;
                if (oneSecondTimer != null)
                {
                    oneSecondTimer.Tick += value2;
                }
            }
        }

        internal virtual TextBox Scoreboard { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

        internal virtual Label Status { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

        internal virtual Button BtnRestore
        {
            [CompilerGenerated] get { return _btnRestore; }
            [CompilerGenerated]
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                EventHandler value2 = btnRestore_Click;
                Button btnRestore = _btnRestore;
                if (btnRestore != null)
                {
                    btnRestore.Click -= value2;
                }
                _btnRestore = value;
                btnRestore = _btnRestore;
                if (btnRestore != null)
                {
                    btnRestore.Click += value2;
                }
            }
        }

        internal virtual OpenFileDialog GetFile { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

        public Form1()
        {
            Load += Form1_Load;
            Resize += Form1_Resize;
            FormClosing += Form1_FormClosing;
            RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
            _area = new int[1001, 1001];
            _distance = new int[201, 201];
            _inuse = new bool[201];
            _active = new bool[201];
            _studentId = new string[201];
            _studentFirstname = new string[201];
            _studentFamilyname = new string[201];
            _ipAddress = new string[201];
            _shipType = new int[201];
            _destinationAddress = new string[201];
            _sourceAddress = new string[201];
            _message = new string[201];
            _displayId = new string[201];
            _displayScore = new int[201];
            _shipX = new int[201];
            _shipY = new int[201];
            _moves = new bool[201];
            _speedX = new int[201];
            _speedY = new int[201];
            _fire = new bool[201];
            _fireX = new int[201];
            _fireY = new int[201];
            _fireCount = new int[201];
            _health = new int[201];
            _newHealth = new int[201];
            _flag = new int[201];
            _score = new int[201];
            _marks = new int[201];
            _commandCount = new int[201];
            _listOfShips = new int[201];
            InitializeComponent();
        }

        private int VisibleRange(int fromShip, int toShip)
        {
            var num = _shipType[fromShip];
            var num2 = _shipType[toShip];
            int result;
            switch (num)
            {
                case ShiptypeBattleship:
                    switch (num2)
                    {
                        case ShiptypeBattleship:
                            result = 200;
                            break;
                        case ShiptypeFrigate:
                            result = 200;
                            break;
                        case ShiptypeSubmarine:
                            result = 5;
                            break;
                        default:
                            result = 200;
                            break;
                    }
                    break;
                case ShiptypeFrigate:
                    switch (num2)
                    {
                        case ShiptypeBattleship:
                            result = 150;
                            break;
                        case ShiptypeFrigate:
                            result = 150;
                            break;
                        case ShiptypeSubmarine:
                            result = 100;
                            break;
                        default:
                            result = 150;
                            break;
                    }
                    break;
                case ShiptypeSubmarine:
                    switch (num2)
                    {
                        case ShiptypeBattleship:
                            result = 100;
                            break;
                        case ShiptypeFrigate:
                            result = 100;
                            break;
                        case ShiptypeSubmarine:
                            result = 100;
                            break;
                        default:
                            result = 100;
                            break;
                    }
                    break;
                default:
                    result = 0;
                    break;
            }
            return result;
        }

        private int FiringRange(int fromShip)
        {
            int result;
            switch (_shipType[fromShip])
            {
                case ShiptypeBattleship:
                    result = 100;
                    break;
                case ShiptypeFrigate:
                    result = 75;
                    break;
                case ShiptypeSubmarine:
                    result = 50;
                    break;
                default:
                    result = 10;
                    break;
            }
            return result;
        }

        private int Damage(int fromShip, int toShip)
        {
            var num = _shipType[fromShip];
            var num2 = _shipType[toShip];
            int result;
            switch (num)
            {
                case ShiptypeBattleship:
                    switch (num2)
                    {
                        case ShiptypeBattleship:
                            result = 1;
                            break;
                        case ShiptypeFrigate:
                            result = 2;
                            break;
                        case ShiptypeSubmarine:
                            result = 3;
                            break;
                        default:
                            result = 5;
                            break;
                    }
                    break;
                case ShiptypeFrigate:
                    switch (num2)
                    {
                        case ShiptypeBattleship:
                            result = 1;
                            break;
                        case ShiptypeFrigate:
                            result = 1;
                            break;
                        case ShiptypeSubmarine:
                            result = 2;
                            break;
                        default:
                            result = 5;
                            break;
                    }
                    break;
                case ShiptypeSubmarine:
                    switch (num2)
                    {
                        case ShiptypeBattleship:
                            result = 2;
                            break;
                        case ShiptypeFrigate:
                            result = 2;
                            break;
                        case ShiptypeSubmarine:
                            result = 2;
                            break;
                        default:
                            result = 5;
                            break;
                    }
                    break;
                default:
                    result = 0;
                    break;
            }
            return result;
        }

        private void initialise_ships()
        {
            _tenMinuteCounter = 0L;
            _showMarks = false;
            var num = 0;
            checked
            {
                do
                {
                    _inuse[num] = false;
                    _active[num] = false;
                    _studentId[num] = "";
                    _studentFirstname[num] = "";
                    _studentFamilyname[num] = "";
                    _ipAddress[num] = "";
                    _destinationAddress[num] = "";
                    _sourceAddress[num] = "";
                    _message[num] = "";
                    _shipType[num] = ShiptypeBattleship;
                    _shipX[num] = 0;
                    _shipY[num] = 0;
                    _moves[num] = false;
                    _speedX[num] = 0;
                    _speedY[num] = 0;
                    _fire[num] = false;
                    _fireX[num] = 0;
                    _fireY[num] = 0;
                    _fireCount[num] = 0;
                    _health[num] = 0;
                    _newHealth[num] = 0;
                    _flag[num] = 0;
                    _score[num] = 0;
                    _marks[num] = 0;
                    _commandCount[num] = 0;
                    num++;
                } while (num <= MaxShips);
                _packX = 500;
                _packY = 500;
                _packTimer = 50;
            }
        }

        private void AddShip(string id, string firstname, string familyname, string ip, int x, int y, int shiptype)
        {
            var num = -1;
            var flag = false;
            var num2 = 0;
            checked
            {
                do
                {
                    if (_ipAddress[num2].Equals(ip))
                    {
                        num = num2;
                        flag = true;
                    }
                    num2++;
                } while (num2 <= MaxShips);
                if (num < 0)
                {
                    num2 = MaxShips;
                    do
                    {
                        if (!_inuse[num2])
                        {
                            num = num2;
                        }
                        num2 += -1;
                    } while (num2 >= 0);
                }
                if (num >= 0 & num <= MaxShips)
                {
                    _inuse[num] = true;
                    _active[num] = true;
                    _studentId[num] = id;
                    _studentFirstname[num] = firstname;
                    _studentFamilyname[num] = familyname;
                    _ipAddress[num] = ip;
                    _shipType[num] = shiptype % 3;
                    _destinationAddress[num] = "";
                    _sourceAddress[num] = "";
                    _message[num] = "";
                    _shipX[num] = x;
                    _shipY[num] = y;
                    _moves[num] = false;
                    _speedX[num] = 0;
                    _speedY[num] = 0;
                    _fire[num] = false;
                    _fireX[num] = 0;
                    _fireY[num] = 0;
                    _health[num] = 10;
                }

                if (!flag)
                    return;

                var num3 = 0;
                num2 = 0;
                do
                {
                    if (_inuse[num2])
                    {
                        _score[num2] = _score[num2] + 1;
                        num3++;
                    }
                    num2++;
                } while (num2 <= MaxShips);
                _score[num] = _score[num] - num3;
            }
        }

        private string GetIp(string id)
        {
            var num = -1;
            var num2 = 0;
            checked
            {
                do
                {
                    if (_studentId[num2].Equals(id))
                    {
                        num = num2;
                    }
                    num2++;
                } while (num2 <= MaxShips);
                string result;
                if (num >= 0 & num <= MaxShips)
                {
                    result = _ipAddress[num];
                }
                else
                {
                    result = "";
                }
                return result;
            }
        }

        private void AddMessage(string id, string ip, string dest, string srce, string msg)
        {
            var num = -1;
            var num2 = 0;
            checked
            {
                do
                {
                    if (_studentId[num2].Equals(id) && _ipAddress[num2].Equals(ip))
                    {
                        num = num2;
                    }
                    num2++;
                } while (num2 <= MaxShips);

                if (!(num >= 0 & num <= MaxShips))
                    return;

                _inuse[num] = true;
                _active[num] = true;
                _destinationAddress[num] = dest;
                _sourceAddress[num] = srce;
                _message[num] = msg;
                if (_commandCount[num] < 22)
                {
                    _commandCount[num] = _commandCount[num] + 1;
                }
            }
        }

        private void AddFire(string id, string ip, int x, int y)
        {
            var num = -1;
            var num2 = 0;
            checked
            {
                do
                {
                    if (_studentId[num2].Equals(id) && _ipAddress[num2].Equals(ip))
                    {
                        num = num2;
                    }
                    num2++;
                } while (num2 <= MaxShips);

                if (!(num >= 0 & num <= MaxShips))
                    return;

                _inuse[num] = true;
                _active[num] = true;
                _fire[num] = true;
                _fireX[num] = x;
                _fireY[num] = y;
                if (_commandCount[num] < 22)
                {
                    _commandCount[num] = _commandCount[num] + 1;
                }
            }
        }

        private void Addmoves(string id, string ip, int x, int y)
        {
            var num = -1;
            var num2 = 0;
            checked
            {
                do
                {
                    if (_studentId[num2].Equals(id) && _ipAddress[num2].Equals(ip))
                    {
                        num = num2;
                    }
                    num2++;
                } while (num2 <= MaxShips);

                if (!(num >= 0 & num <= MaxShips))
                    return;

                _inuse[num] = true;
                _active[num] = true;
                _moves[num] = true;
                _speedX[num] = x;
                _speedY[num] = y;
                if (_commandCount[num] < 22)
                {
                    _commandCount[num] = _commandCount[num] + 1;
                }
            }
        }

        private void Addflag(string id, string ip, int newFlag)
        {
            var num = -1;
            var num2 = 0;
            checked
            {
                do
                {
                    if (_studentId[num2].Equals(id) &
                        _ipAddress[num2].Equals(ip))
                    {
                        num = num2;
                    }
                    num2++;
                } while (num2 <= MaxShips);

                if (!(num >= 0 & num <= MaxShips))
                    return;

                _inuse[num] = true;
                _active[num] = true;
                _flag[num] = newFlag;
                if (_commandCount[num] < 22)
                {
                    _commandCount[num] = _commandCount[num] + 1;
                }
            }
        }

        private void AddServerShip(int index, string name, int x, int y, int shipType)
        {
            if (!(index >= 0 & index <= MaxShips))
                return;

            _inuse[index] = true;
            _active[index] = true;
            _studentId[index] = name;
            _studentFirstname[index] = "XXX";
            _studentFamilyname[index] = "XXX";
            _ipAddress[index] = "";
            _destinationAddress[index] = "";
            _sourceAddress[index] = "";
            _message[index] = "";
            _shipType[index] = shipType % 3;
            _shipX[index] = x;
            _shipY[index] = y;
            _speedX[index] = 0;
            _speedY[index] = 0;
            _fire[index] = false;
            _fireX[index] = 0;
            _fireY[index] = 0;
            _health[index] = 10;
        }

        private void UpdateNastyShips(int index)
        {
            checked
            {
                if (!(index >= 0 & index < MaxNasties))
                    return;

                if (_speedX[index] == 0 & _speedX[index] == 0)
                {
                    _speedX[index] = (int) Math.Round(10f * _random.NextDouble() + 1f) - 5;
                    _speedY[index] = (int) Math.Round(10f * _random.NextDouble() + 1f) - 5;
                }
                if (_shipX[index] > 990)
                {
                    _speedX[index] = -2;
                }
                if (_shipX[index] < 10)
                {
                    _speedX[index] = 2;
                }
                if (_shipY[index] > 990)
                {
                    _speedY[index] = -2;
                }
                if (_shipY[index] < 10)
                {
                    _speedY[index] = 2;
                }

                var num = -1;
                var num2 = 1000;
                var num3 = 0;

                do
                {
                    if (_inuse[num3] & _active[num3] &&
                        num3 != index &&
                        _distance[index, num3] < VisibleRange(index, num3) &&
                        num2 > _distance[index, num3])
                    {
                        num2 = _distance[index, num3];
                        num = num3;
                    }
                    num3++;
                } while (num3 <= MaxShips);

                _moves[index] = true;

                if (!(num >= 0 & num <= MaxShips))
                    return;

                if (_fireCount[index] < FireLimit)
                {
                    _fire[index] = true;
                    _fireX[index] = _shipX[num];
                    _fireY[index] = _shipY[num];
                    _fireCount[index] = _fireCount[index] + 1;
                }
                else
                {
                    _fireCount[index] = 1;
                }
                if (_shipX[index] < _shipX[num])
                {
                    _speedX[index] = 2;
                }
                if (_shipX[index] > _shipX[num])
                {
                    _speedX[index] = -2;
                }
                if (_shipY[index] < _shipY[num])
                {
                    _speedY[index] = 2;
                }
                if (_shipY[index] > _shipY[num])
                {
                    _speedY[index] = -2;
                }
                _flag[index] = _flag[num];
            }
        }

        private void UpdateServerShips(int index)
        {
            checked
            {
                if (!(index >= 0 & index <= MaxShips))
                    return;

                if (_speedX[index] == 0 & _speedX[index] == 0)
                {
                    _speedX[index] = (int) Math.Round(10f * _random.NextDouble() + 1f) - 5;
                    _speedY[index] = (int) Math.Round(10f * _random.NextDouble() + 1f) - 5;
                }
                if (_shipX[index] > 990)
                {
                    _speedX[index] = -2;
                }
                if (_shipX[index] < 10)
                {
                    _speedX[index] = 2;
                }
                if (_shipY[index] > 990)
                {
                    _speedY[index] = -2;
                }
                if (_shipY[index] < 10)
                {
                    _speedY[index] = 2;
                }
                switch (index)
                {
                    case 0:
                    {
                        var num = 0;
                        var num2 = -1;
                        var num3 = 1000;
                        var num4 = 0;

                        do
                        {
                            if (_inuse[num4] & _active[num4])
                            {
                                if (num4 != 0 &&
                                    (_shipX[0] < 500 & num4 != 4 & num4 != 5 & num4 != 6 || _shipX[0] >= 500) &&
                                    _distance[0, num4] < VisibleRange(0, num4) &&
                                    num3 > _distance[0, num4])
                                {
                                    num3 = _distance[0, num4];
                                    num2 = num4;
                                }
                                num++;
                            }
                            num4++;
                        } while (num4 <= MaxShips);

                        if (num2 >= 0 & num2 <= MaxShips)
                        {
                            if (_fireCount[0] < FireLimit)
                            {
                                _fire[0] = true;
                                _fireX[0] = _shipX[num2];
                                _fireY[0] = _shipY[num2];
                                _fireCount[0] = _fireCount[index] + 1;
                            }
                            else
                            {
                                _fireCount[0] = 1;
                            }
                            if (_shipX[0] < _shipX[num2])
                            {
                                _speedX[0] = 2;
                            }
                            if (_shipX[0] > _shipX[num2])
                            {
                                _speedX[0] = -2;
                            }
                            if (_shipY[0] < _shipY[num2])
                            {
                                _speedY[0] = 2;
                            }
                            if (_shipY[0] > _shipY[num2])
                            {
                                _speedY[0] = -2;
                            }
                            _flag[0] = _flag[num2];
                        }
                        _moves[0] = true;

                        if (ServerType == DefaultServer & num > 15)
                        {
                            var num5 = 0;
                            var num6 = 0;
                            var num7 = 0;
                            num4 = 7;

                            do
                            {
                                if (_inuse[num4] && _active[num4])
                                {
                                    if (_shipType[num4] == ShiptypeBattleship)
                                    {
                                        num5++;
                                    }
                                    if (_shipType[num4] == ShiptypeFrigate)
                                    {
                                        num6++;
                                    }
                                    if (_shipType[num4] == ShiptypeSubmarine)
                                    {
                                        num7++;
                                    }
                                }
                                num4++;
                            } while (num4 <= MaxShips);

                            if (num5 > num6 + num7)
                            {
                                _shipType[0] = ShiptypeSubmarine;
                            }
                            if (num6 > num5 + num7)
                            {
                                _shipType[0] = ShiptypeBattleship;
                            }
                            if (num7 > num6 + num5)
                            {
                                _shipType[0] = ShiptypeFrigate;
                            }
                        }
                        break;
                    }
                    case 1:
                    {
                        var num2 = -1;
                        var num3 = 1000;
                        var num4 = 0;

                        do
                        {
                            if (_inuse[num4] && _active[num4] && num4 != index &&
                                _distance[index, num4] < VisibleRange(index, num4) &&
                                num3 > _distance[index, num4])
                            {
                                num3 = _distance[index, num4];
                                num2 = num4;
                            }
                            num4++;
                        } while (num4 <= MaxShips);

                        if (num2 >= 0 & num2 <= MaxShips)
                        {
                            _fire[index] = true;
                            _fireX[index] = _shipX[num2];
                            _fireY[index] = _shipY[num2];
                            if (_distance[index, num2] < (FiringRange(index) * 2) / 3)
                            {
                                if (_shipX[index] < _shipX[num2])
                                {
                                    _speedX[index] = -2;
                                }
                                if (_shipX[index] > _shipX[num2])
                                {
                                    _speedX[index] = 2;
                                }
                                if (_shipY[index] < _shipY[num2])
                                {
                                    _speedY[index] = -2;
                                }
                                if (_shipY[index] > _shipY[num2])
                                {
                                    _speedY[index] = 2;
                                }
                            }
                        }
                        _moves[index] = true;
                        return;
                    }
                    case 2:
                    {
                        var num2 = -1;
                        var num3 = 1000;
                        var num4 = 0;

                        do
                        {
                            if (_inuse[num4] & _active[num4] &&
                                num4 != index &&
                                _distance[index, num4] < VisibleRange(index, num4) &&
                                num3 > _distance[index, num4])
                            {
                                num3 = _distance[index, num4];
                                num2 = num4;
                            }
                            num4++;
                        } while (num4 <= MaxShips);

                        if (num2 >= 0 & num2 <= MaxShips)
                        {
                            _fire[index] = true;
                            _fireX[index] = _shipX[num2];
                            _fireY[index] = _shipY[num2];
                        }
                        if (_speedX[index] == -2)
                        {
                            _speedX[index] = -1;
                        }
                        if (_speedY[index] == -2)
                        {
                            _speedY[index] = -1;
                        }
                        _moves[index] = true;
                        return;
                    }
                    case 3:
                        _moves[index] = true;
                        return;
                    case 4:
                    case 5:
                    case 6:
                    {
                        var num2 = -1;
                        var num3 = 1000;
                        var num4 = 0;
                        do
                        {
                            if (_inuse[num4] & _active[num4] &&
                                num4 != 4 & num4 != 5 & num4 != 6 &&
                                (_shipX[num4] < 500 & num4 != 0 || _shipX[num4] >= 500) &&
                                _distance[index, num4] < VisibleRange(index, num4) &&
                                num3 > _distance[index, num4])
                            {
                                num3 = _distance[index, num4];
                                num2 = num4;
                            }
                            num4++;
                        } while (num4 <= MaxShips);

                        if (num2 >= 0 & num2 <= MaxShips)
                        {
                            _fire[index] = true;
                            _fireX[index] = _shipX[num2];
                            _fireY[index] = _shipY[num2];
                            _flag[index] = _flag[num2];
                        }

                        var num8 = 0;
                        var num9 = 0;
                        var num10 = 0;
                        num4 = 4;

                        do
                        {
                            if (num10 < _distance[index, num4])
                            {
                                num10 = _distance[index, num4];
                            }
                            num8 += _shipX[num4];
                            num9 += _shipY[num4];
                            num4++;
                        } while (num4 <= 6);

                        num8 = (int) Math.Round(num8 / 3.0);
                        num9 = (int) Math.Round(num9 / 3.0);

                        if (num10 > 10)
                        {
                            if (_shipX[index] < num8)
                            {
                                _speedX[index] = 2;
                            }
                            if (_shipX[index] > num8)
                            {
                                _speedX[index] = -2;
                            }
                            if (_shipY[index] < num9)
                            {
                                _speedY[index] = 2;
                            }
                            if (_shipY[index] > num9)
                            {
                                _speedY[index] = -2;
                            }
                        }
                        else
                        {
                            if (num2 >= 0 & num2 <= MaxShips)
                            {
                                if (_shipX[index] < _shipX[num2])
                                {
                                    _speedX[index] = 2;
                                }
                                if (_shipX[index] > _shipX[num2])
                                {
                                    _speedX[index] = -2;
                                }
                                if (_shipY[index] < _shipY[num2])
                                {
                                    _speedY[index] = 2;
                                }
                                if (_shipY[index] > _shipY[num2])
                                {
                                    _speedY[index] = -2;
                                }
                            }
                            else
                            {
                                if (_shipX[index] < _packX + index)
                                {
                                    _speedX[index] = 2;
                                }
                                if (_shipX[index] > _packX + index)
                                {
                                    _speedX[index] = -2;
                                }
                                if (_shipY[index] < _packY + index)
                                {
                                    _speedY[index] = 2;
                                }
                                if (_shipY[index] > _packY + index)
                                {
                                    _speedY[index] = -2;
                                }
                            }
                            num4 = 4;
                            do
                            {
                                if (num4 != index && _health[num4] < 4)
                                {
                                    _fire[index] = true;
                                    _fireX[index] = _shipX[num4];
                                    _fireY[index] = _shipY[num4];
                                }
                                num4++;
                            } while (num4 <= 6);
                        }
                        _moves[index] = true;
                        break;
                    }
                    default:
                        return;
                }
            }
        }

        private void resize_form()
        {
            var height = Height;
            var width = Width;

            checked
            {
                var num = height - 70;
                if (num < 20)
                {
                    num = 20;
                }
                var num2 = width - 200;
                if (num2 < 20)
                {
                    num2 = 20;
                }
                BattleArea.Left = 20;
                BattleArea.Top = 20;
                BattleArea.Height = num;
                BattleArea.Width = num2;
                var num3 = height - 100;
                if (num3 < 20)
                {
                    num3 = 20;
                }
                Scoreboard.Left = 20 + num2 + 20;
                Scoreboard.Top = 20;
                Scoreboard.Height = num3;
                Scoreboard.Width = 140;
                Status.Left = Scoreboard.Left;
                Status.Top = Scoreboard.Top + num3 + 10;
                Status.Width = Scoreboard.Width;
                BtnRestore.Left = Scoreboard.Left + 107;
                BtnRestore.Top = Scoreboard.Top + num3 + 10;
            }
        }

        private int Next_Position()
        {
            return (int) Math.Round(1000f * _random.NextDouble() + 1f);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var str = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0].ToString();
            Text = "Battleships Server (" + str + ")";
            BattleArea.BorderStyle = BorderStyle.Fixed3D;
            _displayNames = false;
            var num = 0;
            checked
            {
                do
                {
                    var num2 = 0;
                    do
                    {
                        _area[num, num2] = -1000;
                        num2++;
                    } while (num2 <= 1000);
                    num++;
                } while (num <= 1000);
                num = 0;

                do
                {
                    var num2 = 0;
                    do
                    {
                        _distance[num, num2] = 10000;
                        num2++;
                    } while (num2 <= MaxShips);
                    num++;
                } while (num <= MaxShips);

                resize_form();
                initialise_ships();

                if (ServerType == DefaultServer)
                {
                    AddServerShip(0, "Marks_24", Next_Position(), Next_Position(), ShiptypeBattleship);
                    AddServerShip(1, "Hit_Run_", Next_Position(), Next_Position(), ShiptypeSubmarine);
                    AddServerShip(2, "Rand_Hit", Next_Position(), Next_Position(), ShiptypeFrigate);
                    AddServerShip(3, "Marks_00", Next_Position(), Next_Position(), ShiptypeBattleship);
                    AddServerShip(4, "Pack_01", Next_Position(), Next_Position(), ShiptypeBattleship);
                    AddServerShip(5, "Pack_02", Next_Position(), Next_Position(), ShiptypeBattleship);
                    AddServerShip(6, "Pack_03", Next_Position(), Next_Position(), ShiptypeFrigate);
                }
                else if (ServerType == NastyServer)
                {
                    num = 0;
                    do
                    {
                        AddServerShip(num, "Nasty_" + num % 3 + "_" + num.ToString().Trim(), Next_Position(),
                            Next_Position(), num % 3);
                        num++;
                    } while (num < MaxNasties);
                }

                Scoreboard.Text = "";
                ReceivingUdpClient = new UdpClient(PortFromClient);
                ThreadReceive = new Thread(ReceiveMessages);
                ThreadReceive.Start();
                OneSecondTimer.Enabled = true;
            }
        }

        private void ReceiveMessages()
        {
            //                ProjectData.ClearProjectError();
            checked
            {
                while (true)
                {
                    var bytes = ReceivingUdpClient.Receive(ref RemoteIpEndPoint);
                    var ip = RemoteIpEndPoint.Address.ToString();
                    var @string = Encoding.ASCII.GetString(bytes);
                    int num3;
                    if (@string.Length - 1 > 150)
                    {
                        IllegalMessage(ip);
                        num3 = 150;
                    }
                    else
                    {
                        num3 = @string.Length - 1;
                    }
                    if (@string.StartsWith("Register"))
                    {
                        var num4 = 0;
                        var text = "";
                        var text2 = "";
                        var text3 = "";
                        var text4 = "";

                        var num5 = num3;
                        for (var i = 10; i <= num5; i++)
                        {
                            var text5 = @string.Substring(i, 1);
                            if (text5.Equals(","))
                            {
                                num4++;
                            }
                            else
                            {
                                switch (num4)
                                {
                                    case 0:
                                        text += text5;
                                        break;
                                    case 1:
                                        text2 += text5;
                                        break;
                                    case 2:
                                        text3 += text5;
                                        break;
                                    default:
                                        text4 += text5;
                                        break;
                                }
                            }
                        }

                        AddShip(text, text2, text3, ip, Next_Position(), Next_Position(),
                            Convert.ToInt32(text4.Trim()));
                    }
                    else
                    {
                        if (@string.StartsWith("Fire"))
                        {
                            var num4 = 0;
                            var text = "";
                            var text6 = "";
                            var text7 = "";
                            var num6 = num3;
                            for (var i = 5; i <= num6; i++)
                            {
                                var text5 = @string.Substring(i, 1);
                                if (text5.Equals(","))
                                {
                                    num4++;
                                }
                                else
                                {
                                    switch (num4)
                                    {
                                        case 0:
                                            text += text5;
                                            break;
                                        case 1:
                                            text6 += text5;
                                            break;
                                        default:
                                            text7 += text5;
                                            break;
                                    }
                                }
                            }
                            if (int.Parse(text6) >= 0 & int.Parse(text6) <= 1000 &
                                int.Parse(text7) >= 0 & int.Parse(text7) <= 1000)
                            {
                                AddFire(text, ip, int.Parse(text6), int.Parse(text7));
                            }
                        }
                        else
                        {
                            if (@string.StartsWith("Move"))
                            {
                                var num4 = 0;
                                var text = "";
                                var text6 = "";
                                var text7 = "";
                                var num7 = num3;
                                for (var i = 5; i <= num7; i++)
                                {
                                    var text5 = @string.Substring(i, 1);
                                    if (text5.Equals(","))
                                    {
                                        num4++;
                                    }
                                    else
                                    {
                                        switch (num4)
                                        {
                                            case 0:
                                                text += text5;
                                                break;
                                            case 1:
                                                text6 += text5;
                                                break;
                                            default:
                                                text7 += text5;
                                                break;
                                        }
                                    }
                                }
                                if (int.Parse(text6) >= -2 & int.Parse(text6) <= 2 &
                                    int.Parse(text7) >= -2 & int.Parse(text7) <= 2)
                                {
                                    Addmoves(text, ip, int.Parse(text6),
                                        int.Parse(text7));
                                }
                            }
                            else
                            {
                                if (@string.StartsWith("Message"))
                                {
                                    var num4 = 0;
                                    var text = "";
                                    var text8 = "";
                                    var text9 = "";
                                    var text10 = "";
                                    var num8 = num3;
                                    for (var i = 8; i <= num8; i++)
                                    {
                                        var text5 = @string.Substring(i, 1);
                                        if (text5.Equals(","))
                                        {
                                            num4++;
                                        }
                                        else
                                        {
                                            switch (num4)
                                            {
                                                case 0:
                                                    text += text5;
                                                    break;
                                                case 1:
                                                    text8 += text5;
                                                    break;
                                                case 2:
                                                    text9 += text5;
                                                    break;
                                                default:
                                                    text10 += text5;
                                                    break;
                                            }
                                        }
                                    }
                                    AddMessage(text, ip, text8, text9, text10);
                                }
                                else
                                {
                                    if (@string.StartsWith("Flag"))
                                    {
                                        var num4 = 0;
                                        var text = "";
                                        var text11 = "";
                                        var num9 = num3;
                                        for (var i = 5; i <= num9; i++)
                                        {
                                            var text5 = @string.Substring(i, 1);
                                            if (text5.Equals(","))
                                            {
                                                num4++;
                                            }
                                            else
                                            {
                                                if (num4 == 0)
                                                {
                                                    text += text5;
                                                }
                                                else
                                                {
                                                    text11 += text5;
                                                }
                                            }
                                        }
                                        Addflag(text, ip, int.Parse(text11));
                                    }
                                    else
                                    {
                                        IllegalMessage(ip);
                                    }
                                }
                            }
                        }
                    }
                    Application.DoEvents();
                }
            }
        }

        private static void IllegalMessage(string ip)
        {
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            resize_form();
        }

        private void BattleArea_Click(object sender, EventArgs e)
        {
            _displayNames ^= true;
        }

        private void OneSecondTimer_Tick(object sender, EventArgs e)
        {
            Image image = new Bitmap(BattleArea.Width, BattleArea.Height);
            var graphics = Graphics.FromImage(image);
            var udpClient = new UdpClient();
            //                ProjectData.ClearProjectError();
            OneSecondTimer.Enabled = false;
            checked
            {
                _packTimer--;
                if (_packTimer > 0)
                {
                    goto IL_104;
                }
                _packX = (int) Math.Round(800f * _random.NextDouble() + 100f);
                if (_packX <= 900)
                {
                    goto IL_B2;
                }
                _packX = 900;
                IL_B2:
                _packY = (int) Math.Round(800f * _random.NextDouble() + 100f);
                if (_packY <= 900)
                {
                    goto IL_F6;
                }
                _packY = 900;
                IL_F6:
                _packTimer = 180;
                IL_104:
                var i = 0;

                do
                {
                    _newHealth[i] = _health[i];
                    Application.DoEvents();
                    i++;
                } while (i <= MaxShips);

                i = 0;

                do
                {
                    if (_inuse[i] & _active[i] & _fire[i])
                    {
                        var j = 0;
                        do
                        {
                            if (_inuse[j] && // Ship is in use.
                                _active[j] && // Ship is active.
                                i != j && // Ship is not the one that's firing.
                                _distance[i, j] <= FiringRange(i) && // Ship is within firing range.
                                _fireX[i] == _shipX[j] && // Target X = Ship X
                                _fireY[i] == _shipY[j] && // Target Y = Ship Y

                                // Generate shot chance, factoring in distance, health and chance.
                                _distance[i, j] < ((((FiringRange(i) * _health[i]) * _random.NextDouble()) / 8) + 1))
                            {
                                if (_score[i] < 32000)
                                {
                                    _score[i] += Damage(i, j);
                                }
                                if (_score[j] > -32000)
                                {
                                    _score[j] -= Damage(i, j);
                                }
                                _newHealth[j] -= Damage(i, j);
                            }
                            j++;
                        } while (j <= MaxShips);
                    }

                    _fire[i] = false;
                    Application.DoEvents();
                    i++;
                } while (i <= MaxShips);

                i = 0;

                do
                {
                    _health[i] = _newHealth[i];
                    if (_inuse[i] & _active[i])
                    {
                        if (_health[i] <= 0)
                        {
                            _shipX[i] = Next_Position();
                            _shipY[i] = Next_Position();
                            _speedX[i] = (int) Math.Round(5f * _random.NextDouble() - 2f);
                            _speedY[i] = (int) Math.Round(5f * _random.NextDouble() - 2f);
                            _health[i] = 10;
                        }
                    }
                    Application.DoEvents();
                    i++;
                } while (i <= MaxShips);

                i = 0;

                do
                {
                    if (_inuse[i] & _active[i] & _moves[i])
                    {
                        if (_speedX[i] > 2)
                        {
                            _speedX[i] = 2;
                        }
                        if (_speedX[i] < -2)
                        {
                            _speedX[i] = -2;
                        }
                        if (_speedY[i] > 2)
                        {
                            _speedY[i] = 2;
                        }
                        if (_speedY[i] < -2)
                        {
                            _speedY[i] = -2;
                        }
                        var num3 = (float) (_health[i] * _speedX[i] / 10.0);
                        var num4 = (float) (_health[i] * _speedY[i] / 10.0);
                        _shipX[i] = (int) Math.Round(_shipX[i] + num3);
                        _shipY[i] = (int) Math.Round(_shipY[i] + num4);
                        if (_shipX[i] < 2)
                        {
                            _shipX[i] = 2;
                        }
                        if (_shipX[i] > 995)
                        {
                            _shipX[i] = 995;
                        }
                        if (_shipY[i] < 5)
                        {
                            _shipY[i] = 5;
                        }
                        if (_shipY[i] > 995)
                        {
                            _shipY[i] = 995;
                        }
                    }
                    _moves[i] = false;
                    Application.DoEvents();
                    i++;
                } while (i <= MaxShips);

                var font = new Font("Verdana", 8f);
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.FillRegion(Brushes.LightCyan, new Region(BattleArea.ClientRectangle));
                i = 0;

                do
                {
                    float num5;
                    float num6;
                    unchecked
                    {
                        num5 = (float) (checked((BattleArea.Width - 5) * _shipX[i]) / 1000.0 - 2.0);
                        num6 = (float) (BattleArea.Height - checked(BattleArea.Height * _shipY[i]) / 1000.0 - 2.0);
                    }
                    if (_inuse[i] & _active[i])
                    {
                        switch (_shipType[i])
                        {
                            case ShiptypeBattleship:
                                switch (_health[i])
                                {
                                    case 1:
                                    case 2:
                                        graphics.FillRectangle(Brushes.OrangeRed, num5, num6, 5f, 5f);
                                        break;
                                    case 3:
                                    case 4:
                                        graphics.FillRectangle(Brushes.Orange, num5, num6, 5f, 5f);
                                        break;
                                    case 5:
                                    case 6:
                                        graphics.FillRectangle(Brushes.Yellow, num5, num6, 5f, 5f);
                                        break;
                                    case 7:
                                    case 8:
                                        graphics.FillRectangle(Brushes.GreenYellow, num5, num6, 5f, 5f);
                                        break;
                                    case 9:
                                    case 10:
                                        graphics.FillRectangle(Brushes.Green, num5, num6, 5f, 5f);
                                        break;
                                    default:
                                        graphics.FillRectangle(Brushes.Red, num5, num6, 5f, 5f);
                                        break;
                                }
                                break;
                            case ShiptypeFrigate:
                                switch (_health[i])
                                {
                                    case 1:
                                    case 2:
                                        graphics.FillEllipse(Brushes.OrangeRed, num5, num6, 5f, 5f);
                                        break;
                                    case 3:
                                    case 4:
                                        graphics.FillEllipse(Brushes.Orange, num5, num6, 5f, 5f);
                                        break;
                                    case 5:
                                    case 6:
                                        graphics.FillEllipse(Brushes.Yellow, num5, num6, 5f, 5f);
                                        break;
                                    case 7:
                                    case 8:
                                        graphics.FillEllipse(Brushes.GreenYellow, num5, num6, 5f, 5f);
                                        break;
                                    case 9:
                                    case 10:
                                        graphics.FillEllipse(Brushes.Green, num5, num6, 5f, 5f);
                                        break;
                                    default:
                                        graphics.FillEllipse(Brushes.Red, num5, num6, 5f, 5f);
                                        break;
                                }
                                break;
                            case ShiptypeSubmarine:
                                switch (_health[i])
                                {
                                    case 1:
                                    case 2:
                                        graphics.FillEllipse(Brushes.OrangeRed, num5, num6, 7f, 3f);
                                        break;
                                    case 3:
                                    case 4:
                                        graphics.FillEllipse(Brushes.Orange, num5, num6, 7f, 3f);
                                        break;
                                    case 5:
                                    case 6:
                                        graphics.FillEllipse(Brushes.Yellow, num5, num6, 7f, 3f);
                                        break;
                                    case 7:
                                    case 8:
                                        graphics.FillEllipse(Brushes.GreenYellow, num5, num6, 7f, 3f);
                                        break;
                                    case 9:
                                    case 10:
                                        graphics.FillEllipse(Brushes.Green, num5, num6, 7f, 3f);
                                        break;
                                    default:
                                        graphics.FillEllipse(Brushes.Red, num5, num6, 7f, 3f);
                                        break;
                                }
                                break;
                        }
                        if (_displayNames)
                        {
                            var p = new Point
                            {
                                X = (int) Math.Round(num5 + 5f),
                                Y = (int) Math.Round(num6)
                            };
                            graphics.DrawString(_studentId[i], font, Brushes.DarkGreen, p);
                        }
                    }
                    Application.DoEvents();
                    i++;
                } while (i <= MaxShips);

                BattleArea.Image = image;
                graphics.Dispose();
                var text = "";
                var num7 = 0;
                Set_Marks();
                i = 0;

                do
                {
                    if (_inuse[i] & _active[i])
                    {
                        _displayId[num7] = _studentId[i];
                        if (_showMarks)
                        {
                            _displayScore[num7] = _marks[i];
                        }
                        else
                        {
                            _displayScore[num7] = _score[i];
                        }
                        num7++;
                    }
                    i++;
                } while (i <= MaxShips);

                var num8 = num7 - 1;

                for (i = 0; i <= num8; i++)
                {
                    var num9 = num7 - 1;
                    for (var j = 0; j <= num9; j++)
                    {
                        if (_displayScore[i] <= _displayScore[j])
                            continue;

                        var text2 = _displayId[i];
                        var num10 = _displayScore[i];
                        _displayId[i] = _displayId[j];
                        _displayScore[i] = _displayScore[j];
                        _displayId[j] = text2;
                        _displayScore[j] = num10;
                    }
                }

                var num11 = num7 - 1;

                for (i = 0; i <= num11; i++)
                {
                    text = string.Concat(text, _displayId[i], " ", _displayScore[i].ToString(), "\r\n");
                }

                Scoreboard.Text = text;
                i = 0;

                do
                {
                    var j = 0;

                    do
                    {
                        double num12 = _shipX[i] - _shipX[j];
                        double num13 = _shipY[i] - _shipY[j];
                        double d = unchecked(num12 * num12 + num13 * num13);
                        _distance[i, j] = (int) Math.Round(Math.Sqrt(d));
                        j++;
                    } while (j <= MaxShips);

                    Application.DoEvents();
                    i++;
                } while (i <= MaxShips);

                if (ServerType == DefaultServer)
                {
                    UpdateServerShips(0);
                    UpdateServerShips(1);
                    UpdateServerShips(2);
                    UpdateServerShips(3);
                    UpdateServerShips(4);
                    UpdateServerShips(5);
                    UpdateServerShips(6);
                }
                else if (ServerType == NastyServer)
                {
                    i = 0;
                    do
                    {
                        UpdateNastyShips(i);
                        i++;
                    } while (i < MaxNasties);
                }

                var num14 = 0;

                switch (ServerType)
                {
                    case DefaultServer:
                        num14 = 7;
                        break;
                    case NastyServer:
                        num14 = MaxNasties;
                        break;
                    case ZeroServer:
                        num14 = 0;
                        break;
                }

                for (i = num14; i <= MaxShips; i++)
                {
                    if (!(_inuse[i] & _active[i]))
                        continue;

                    var text3 = string.Concat(
                        _shipX[i].ToString(),
                        ",",
                        _shipY[i].ToString(),
                        ",",
                        _health[i].ToString(),
                        ",",
                        _flag[i].ToString(),
                        ",",
                        _shipType[i].ToString()
                    );
                    _numberOfShips = 0;
                    var j = 0;

                    do
                    {
                        if (((_inuse[j] & _active[j] & i != j) &&
                             (_distance[i, j] <= VisibleRange(i, j))))
                        {
                            _listOfShips[_numberOfShips] = j;
                            _numberOfShips++;
                        }
                        j++;
                    } while (j <= MaxShips);

                    if (_numberOfShips > 1)
                    {
                        var num15 = _numberOfShips - 1;
                        for (j = 0; j <= num15; j++)
                        {
                            var num16 = (int) Math.Round(_numberOfShips * _random.NextDouble());
                            var num17 = (int) Math.Round(_numberOfShips * _random.NextDouble() + 1f);

                            if (!(num16 < _numberOfShips & num17 < _numberOfShips))
                                continue;

                            var num18 = _listOfShips[num16];
                            _listOfShips[num16] = _listOfShips[num17];
                            _listOfShips[num17] = num18;
                        }
                    }

                    if (_numberOfShips > 0)
                    {
                        var num19 = _numberOfShips - 1;
                        for (j = 0; j <= num19; j++)
                        {
                            text3 = string.Concat(text3, "|",
                                _shipX[_listOfShips[j]].ToString(), ",",
                                _shipY[_listOfShips[j]].ToString(), ",",
                                _health[_listOfShips[j]].ToString(), ",",
                                _flag[_listOfShips[j]].ToString(), ",",
                                _shipType[_listOfShips[j]].ToString());
                        }
                    }

                    text3 += "\0";
                    var addr = IPAddress.Parse(_ipAddress[i]);
                    var port = PortToClient;

                    udpClient.Connect(addr, port);

                    var array = Encoding.ASCII.GetBytes(text3);
                    var arg12690 = udpClient;
                    var expr1266 = array;

                    arg12690.Send(expr1266, expr1266.Length);

                    Application.DoEvents();

                    if (!_destinationAddress[i].Equals("") &
                        !_message[i].Equals("") &
                        !GetIp(_destinationAddress[i]).Equals("") &
                        _destinationAddress[i].Length > 4)
                    {
                        var s = string.Concat("Message ", _destinationAddress[i], ", ", _sourceAddress[i],
                            ", ", _message[i], "\0");
                        addr = IPAddress.Parse(GetIp(_destinationAddress[i]));
                        port = PortToClient;
                        udpClient.Connect(addr, port);
                        array = Encoding.ASCII.GetBytes(s);
                        var arg13960 = udpClient;
                        var expr1393 = array;
                        arg13960.Send(expr1393, expr1393.Length);
                        _destinationAddress[i] = "";
                        _message[i] = "";
                    }

                    Application.DoEvents();
                }

                udpClient.Close();
                _tenMinuteCounter += 1L;

                if (_tenMinuteCounter < 6000L)
                {
                    goto IL_144A;
                }

                if (!(TutorVersion & ServerType == DefaultServer)) // Currently always true, no marks to be printed.
                {
                    goto IL_143C;
                }

                Print_Marks();
                IL_143C:
                _tenMinuteCounter = 0L;
                IL_144A:
                i = 0;

                do
                {
                    if (_commandCount[i] > MaxAllowedCommands)
                    {
                        IllegalMessage(_ipAddress[i]);
                    }
                    _commandCount[i] = 0;
                    i++;
                } while (i <= MaxShips);

                OneSecondTimer.Interval = 95;
                OneSecondTimer.Enabled = true;
            }
        }

        private void Set_Marks()
        {
            var num = _score[3];
            var num2 = 4;
            checked
            {
                do
                {
                    if (_inuse[num2] && _active[num2] && num < _score[num2])
                    {
                        num = _score[num2];
                    }
                    num2++;
                } while (num2 <= MaxShips);

                var num3 = _score[0];
                var num4 = _score[3];
                num2 = 0;

                do
                {
                    if (_inuse[num2] & _active[num2])
                    {
                        if (_score[num2] <= num3)
                        {
                            if (num3 - num4 > 0)
                            {
                                _marks[num2] = (int) Math.Round(24 * (_score[num2] - num4) / (double) (num3 - num4));
                            }
                            else
                            {
                                _marks[num2] = 0;
                            }
                            if (_marks[num2] > 24)
                            {
                                _marks[num2] = 24;
                            }
                            if (_marks[num2] < 0)
                            {
                                _marks[num2] = 0;
                            }
                        }
                        else
                        {
                            if (num - num3 > 0)
                            {
                                _marks[num2] =
                                    (int) Math.Round(checked(36 * (_score[num2] - num3) / (double) (num - num3)) +
                                                     24.0);
                            }
                            else
                            {
                                _marks[num2] = 0;
                            }
                            if (_marks[num2] > MaxMarks)
                            {
                                _marks[num2] = MaxMarks;
                            }
                            if (_marks[num2] < 25)
                            {
                                _marks[num2] = 25;
                            }
                        }
                    }
                    num2++;
                } while (num2 <= MaxShips);
            }
        }

        private void Print_Marks()
        {
            var streamWriter = new StreamWriter(DateTime.Now.ToString("ddd_HH_mm_MMM_d") + "_Attendance.txt");
            Set_Marks();
            var num = 0;
            checked
            {
                do
                {
                    if (_inuse[num] & _active[num])
                    {
                        var value = string.Concat(_studentId[num], ",  ",
                            _studentFamilyname[num], ",   ",
                            _studentFirstname[num], ",   ",
                            _ipAddress[num], ",   ",
                            _score[num].ToString(), ",   ",
                            _marks[num].ToString(), ",   ",
                            _shipType[num].ToString());
                        streamWriter.WriteLine(value);
                    }
                    num++;
                } while (num <= MaxShips);

                streamWriter.Close();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (TutorVersion & ServerType == DefaultServer) // Currently always true, no marks to be printed.
            {
                Print_Marks();
            }
            ThreadReceive.Abort();
            ReceivingUdpClient.Close();
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            OneSecondTimer.Enabled = false;
            initialise_ships();
            _numberOfShips = 0;
            openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() != DialogResult.OK)
            {
            }
            var fileName = openFileDialog.FileName;
            checked
            {
                using (var streamReader = new StreamReader(fileName, true))
                {
                    var num3 = 0;
                    var text = streamReader.ReadLine();

                    while (text != null)
                    {
                        if (text.Length <= 5)
                            continue;

                        var num4 = 0;
                        var text2 = "";
                        var text3 = "";
                        var text4 = "";
                        var text5 = "";
                        var text6 = "";
                        var text7 = "";
                        var text8 = "";
                        var num5 = text.Length - 1;

                        for (var i = 0; i <= num5; i++)
                        {
                            var text9 = text.Substring(i, 1);
                            if (text9.Equals(","))
                            {
                                num4++;
                            }
                            else if (!text9.Equals("\r"))
                            {
                                switch (num4)
                                {
                                    case 0:
                                        text2 += text9;
                                        break;
                                    case 1:
                                        text3 += text9;
                                        break;
                                    case 2:
                                        text4 += text9;
                                        break;
                                    case 3:
                                        text5 += text9;
                                        break;
                                    case 4:
                                        text6 += text9;
                                        break;
                                    case 5:
                                        text7 += text9;
                                        break;
                                    default:
                                        text8 += text9;
                                        break;
                                }
                            }
                        }

                        if (num3 >= 0 & num3 <= MaxShips)
                        {
                            _inuse[num3] = true;
                            _active[num3] = true;
                            _studentId[num3] = text2.Trim();
                            _studentFirstname[num3] = text3.Trim();
                            _studentFamilyname[num3] = text4.Trim();
                            _ipAddress[num3] = text5.Trim();
                            _destinationAddress[num3] = "";
                            _sourceAddress[num3] = "";
                            _message[num3] = "";
                            _shipX[num3] = Next_Position();
                            _shipY[num3] = Next_Position();
                            _moves[num3] = false;
                            _speedX[num3] = 0;
                            _speedY[num3] = 0;
                            _fire[num3] = false;
                            _fireX[num3] = 0;
                            _fireY[num3] = 0;
                            _health[num3] = 10;
                            _score[num3] = Convert.ToInt32(text6.Trim());
                            _marks[num3] = Convert.ToInt32(text7.Trim());
                            _shipType[num3] = Convert.ToInt32(text8.Trim());
                        }
                        text = streamReader.ReadLine();
                        num3++;
                    }
                    _numberOfShips = num3;
                    streamReader.Close();
                }
                OneSecondTimer.Enabled = true;
            }
        }

        private void BattleArea_DoubleClick(object sender, EventArgs e)
        {
            _showMarks ^= true;
            _displayNames ^= true;
        }

        [DebuggerNonUserCode]
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    components?.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        [DebuggerStepThrough]
        private void InitializeComponent()
        {
            components = new Container();
            BattleArea = new PictureBox();
            OneSecondTimer = new Timer(components);
            Scoreboard = new TextBox();
            Status = new Label();
            BtnRestore = new Button();
            GetFile = new OpenFileDialog();
            ((ISupportInitialize) BattleArea).BeginInit();
            SuspendLayout();
            BattleArea.AccessibleName = "";
            BattleArea.BackColor = Color.LightCyan;
            BattleArea.Location = new Point(18, 20);
            BattleArea.Name = "BattleArea";
            BattleArea.Size = new Size(354, 342);
            BattleArea.TabIndex = 0;
            BattleArea.TabStop = false;
            Scoreboard.AcceptsReturn = true;
            //Scoreboard.BackColor = SystemColors.Info;
            Scoreboard.Enabled = false;
            //Scoreboard.Font = new Font("Verdana", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            Scoreboard.Location = new Point(477, 24);
            Scoreboard.Multiline = true;
            Scoreboard.Name = "Scoreboard";
            Scoreboard.Size = new Size(104, 385);
            Scoreboard.TabIndex = 1;
            Scoreboard.Text = "11111111  88";
            Status.AutoSize = true;
            Status.Location = new Point(481, 422);
            Status.Name = "Status";
            Status.Size = new Size(58, 13);
            Status.TabIndex = 2;
            Status.Text = "Battleships";
            BtnRestore.Location = new Point(547, 421);
            BtnRestore.Name = "BtnRestore";
            BtnRestore.Size = new Size(33, 13);
            BtnRestore.TabIndex = 3;
            BtnRestore.UseVisualStyleBackColor = true;
            GetFile.FileName = "OpenFileDialog1";
            AutoScaleDimensions = new SizeF(6f, 13f);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(596, 458);
            Controls.Add(BtnRestore);
            Controls.Add(Status);
            Controls.Add(Scoreboard);
            Controls.Add(BattleArea);
            Name = "Form1";
            Text = "Battleships";
            ((ISupportInitialize) BattleArea).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}