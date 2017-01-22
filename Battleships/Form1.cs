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

        private const int MAX_MARKS = 60;

        private const int MAX_ALLOWED_COMMANDS = 20;

        private const int SHIPTYPE_BATTLESHIP = 0;

        private const int SHIPTYPE_FRIGATE = 1;

        private const int SHIPTYPE_SUBMARINE = 2;

        private const int MAX_SHIPS = 200;

        private const int FIRING_RANGE = 100;

        private const int VISIBLE_RANGE = 200;

        private const int FIRE_LIMIT = 10;

        private const bool TUTOR_VERSION = false;

        private const int PORT_TO_CLIENT = 1925;

        private const int PORT_FROM_CLIENT = 1924;

        private const int DEFAULT_SERVER = 0;

        private const int NASTY_SERVER = 1;

        private const int ZERO_SERVER = 2;

        private const int MAX_NASTIES = 20;

        private readonly Random _random = new Random();

        private int server_type;

        public UdpClient receivingUdpClient;

        public IPEndPoint RemoteIpEndPoint;

        public Thread ThreadReceive;

        private int SocketNO;

        private int[,] Area;

        private int[,] Distance;

        private bool displayNames;

        private bool[] inuse;

        private bool[] active;

        private string[] student_id;

        private string[] student_firstname;

        private string[] student_familyname;

        private string[] ip_address;

        private int[] ship_type;

        private string[] destination_address;

        private string[] source_address;

        private string[] message;

        private string[] display_id;

        private int[] display_score;

        private int[] shipX;

        private int[] shipY;

        private bool[] moves;

        private int[] speedX;

        private int[] speedY;

        private bool[] fire;

        private int[] fireX;

        private int[] fireY;

        private int[] fireCount;

        private int[] health;

        private int[] new_health;

        private int[] flag;

        private int[] score;

        private int[] marks;

        private int[] command_count;

        private int number_of_ships;

        private int[] list_of_ships;

        private int packX;

        private int packY;

        private int packTimer;

        private long TenMinuteCounter;

        private bool show_marks;

        private IContainer components;

        [AccessedThroughProperty("BattleArea"), CompilerGenerated]
        private PictureBox _BattleArea;

        [AccessedThroughProperty("OneSecondTimer"),
         CompilerGenerated]
        private Timer _OneSecondTimer;

        [AccessedThroughProperty("Scoreboard"), CompilerGenerated]
        private TextBox _Scoreboard;

        [AccessedThroughProperty("Status"), CompilerGenerated]
        private Label _Status;

        [AccessedThroughProperty("btnRestore"), CompilerGenerated]
        private Button _btnRestore;

        [AccessedThroughProperty("getFile"), CompilerGenerated]
        private OpenFileDialog _getFile;

        internal virtual PictureBox BattleArea
        {
            [CompilerGenerated]
            get { return _BattleArea; }
            [CompilerGenerated]
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                EventHandler value2 = BattleArea_Click;
                EventHandler value3 = BattleArea_DoubleClick;
                PictureBox battleArea = _BattleArea;
                if (battleArea != null)
                {
                    battleArea.Click -= value2;
                    battleArea.DoubleClick -= value3;
                }
                _BattleArea = value;
                battleArea = _BattleArea;
                if (battleArea != null)
                {
                    battleArea.Click += value2;
                    battleArea.DoubleClick += value3;
                }
            }
        }

        internal virtual Timer OneSecondTimer
        {
            [CompilerGenerated]
            get { return _OneSecondTimer; }
            [CompilerGenerated]
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                EventHandler value2 = OneSecondTimer_Tick;
                Timer oneSecondTimer = _OneSecondTimer;
                if (oneSecondTimer != null)
                {
                    oneSecondTimer.Tick -= value2;
                }
                _OneSecondTimer = value;
                oneSecondTimer = _OneSecondTimer;
                if (oneSecondTimer != null)
                {
                    oneSecondTimer.Tick += value2;
                }
            }
        }

        internal virtual TextBox Scoreboard { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

        internal virtual Label Status { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

        internal virtual Button btnRestore
        {
            [CompilerGenerated]
            get { return _btnRestore; }
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

        internal virtual OpenFileDialog getFile { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

        public Form1()
        {
            Load += Form1_Load;
            Resize += Form1_Resize;
            FormClosing += Form1_FormClosing;
            RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
            Area = new int[1001, 1001];
            Distance = new int[201, 201];
            inuse = new bool[201];
            active = new bool[201];
            student_id = new string[201];
            student_firstname = new string[201];
            student_familyname = new string[201];
            ip_address = new string[201];
            ship_type = new int[201];
            destination_address = new string[201];
            source_address = new string[201];
            message = new string[201];
            display_id = new string[201];
            display_score = new int[201];
            shipX = new int[201];
            shipY = new int[201];
            moves = new bool[201];
            speedX = new int[201];
            speedY = new int[201];
            fire = new bool[201];
            fireX = new int[201];
            fireY = new int[201];
            fireCount = new int[201];
            health = new int[201];
            new_health = new int[201];
            flag = new int[201];
            score = new int[201];
            marks = new int[201];
            command_count = new int[201];
            list_of_ships = new int[201];
            InitializeComponent();
        }

        private int VisibleRange(int fromShip, int toShip)
        {
            int num = ship_type[fromShip];
            int num2 = ship_type[toShip];
            int result;
            switch (num)
            {
                case 0:
                    switch (num2)
                    {
                        case 0:
                            result = 200;
                            break;
                        case 1:
                            result = 200;
                            break;
                        case 2:
                            result = 5;
                            break;
                        default:
                            result = 200;
                            break;
                    }
                    break;
                case 1:
                    switch (num2)
                    {
                        case 0:
                            result = 150;
                            break;
                        case 1:
                            result = 150;
                            break;
                        case 2:
                            result = 100;
                            break;
                        default:
                            result = 150;
                            break;
                    }
                    break;
                case 2:
                    switch (num2)
                    {
                        case 0:
                            result = 100;
                            break;
                        case 1:
                            result = 100;
                            break;
                        case 2:
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
            switch (ship_type[fromShip])
            {
                case 0:
                    result = 100;
                    break;
                case 1:
                    result = 75;
                    break;
                case 2:
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
            int num = ship_type[fromShip];
            int num2 = ship_type[toShip];
            int result;
            switch (num)
            {
                case 0:
                    switch (num2)
                    {
                        case 0:
                            result = 1;
                            break;
                        case 1:
                            result = 2;
                            break;
                        case 2:
                            result = 3;
                            break;
                        default:
                            result = 5;
                            break;
                    }
                    break;
                case 1:
                    switch (num2)
                    {
                        case 0:
                            result = 1;
                            break;
                        case 1:
                            result = 1;
                            break;
                        case 2:
                            result = 2;
                            break;
                        default:
                            result = 5;
                            break;
                    }
                    break;
                case 2:
                    switch (num2)
                    {
                        case 0:
                            result = 2;
                            break;
                        case 1:
                            result = 2;
                            break;
                        case 2:
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
            server_type = 0;
            TenMinuteCounter = 0L;
            show_marks = false;
            int num = 0;
            checked
            {
                do
                {
                    inuse[num] = false;
                    active[num] = false;
                    student_id[num] = "";
                    student_firstname[num] = "";
                    student_familyname[num] = "";
                    ip_address[num] = "";
                    destination_address[num] = "";
                    source_address[num] = "";
                    message[num] = "";
                    ship_type[num] = 0;
                    shipX[num] = 0;
                    shipY[num] = 0;
                    moves[num] = false;
                    speedX[num] = 0;
                    speedY[num] = 0;
                    fire[num] = false;
                    fireX[num] = 0;
                    fireY[num] = 0;
                    fireCount[num] = 0;
                    health[num] = 0;
                    new_health[num] = 0;
                    flag[num] = 0;
                    score[num] = 0;
                    marks[num] = 0;
                    command_count[num] = 0;
                    num++;
                } while (num <= 200);
                packX = 500;
                packY = 500;
                packTimer = 50;
            }
        }

        private void addShip(string id, string firstname, string familyname, string ip, int X, int Y, int shiptype)
        {
            int num = -1;
            bool flag = false;
            int num2 = 0;
            checked
            {
                do
                {
                    if (ip_address[num2].Equals(ip))
                    {
                        num = num2;
                        flag = true;
                    }
                    num2++;
                } while (num2 <= 200);
                if (num < 0)
                {
                    num2 = 200;
                    do
                    {
                        if (!inuse[num2])
                        {
                            num = num2;
                        }
                        num2 += -1;
                    } while (num2 >= 0);
                }
                if (num >= 0 & num <= 200)
                {
                    inuse[num] = true;
                    active[num] = true;
                    student_id[num] = id;
                    student_firstname[num] = firstname;
                    student_familyname[num] = familyname;
                    ip_address[num] = ip;
                    ship_type[num] = shiptype % 3;
                    destination_address[num] = "";
                    source_address[num] = "";
                    message[num] = "";
                    shipX[num] = X;
                    shipY[num] = Y;
                    moves[num] = false;
                    speedX[num] = 0;
                    speedY[num] = 0;
                    fire[num] = false;
                    fireX[num] = 0;
                    fireY[num] = 0;
                    health[num] = 10;
                }
                if (flag)
                {
                    int num3 = 0;
                    num2 = 0;
                    do
                    {
                        if (inuse[num2])
                        {
                            score[num2] = score[num2] + 1;
                            num3++;
                        }
                        num2++;
                    } while (num2 <= 200);
                    score[num] = score[num] - num3;
                }
            }
        }

        private string getIP(string id)
        {
            int num = -1;
            int num2 = 0;
            checked
            {
                do
                {
                    if (student_id[num2].Equals(id))
                    {
                        num = num2;
                    }
                    num2++;
                } while (num2 <= 200);
                string result;
                if (num >= 0 & num <= 200)
                {
                    result = ip_address[num];
                }
                else
                {
                    result = "";
                }
                return result;
            }
        }

        private void addMessage(string id, string ip, string dest, string srce, string msg)
        {
            int num = -1;
            int num2 = 0;
            checked
            {
                do
                {
                    if (student_id[num2].Equals(id) &&
                        ip_address[num2].Equals(ip))
                    {
                        num = num2;
                    }
                    num2++;
                } while (num2 <= 200);
                if (num >= 0 & num <= 200)
                {
                    inuse[num] = true;
                    active[num] = true;
                    destination_address[num] = dest;
                    source_address[num] = srce;
                    message[num] = msg;
                    if (command_count[num] < 22)
                    {
                        command_count[num] = command_count[num] + 1;
                    }
                }
            }
        }

        private void addFire(string id, string ip, int X, int Y)
        {
            int num = -1;
            int num2 = 0;
            checked
            {
                do
                {
                    if (student_id[num2].Equals(id) &
                        ip_address[num2].Equals(ip))
                    {
                        num = num2;
                    }
                    num2++;
                } while (num2 <= 200);
                if (num >= 0 & num <= 200)
                {
                    inuse[num] = true;
                    active[num] = true;
                    fire[num] = true;
                    fireX[num] = X;
                    fireY[num] = Y;
                    if (command_count[num] < 22)
                    {
                        command_count[num] = command_count[num] + 1;
                    }
                }
            }
        }

        private void addmoves(string id, string ip, int X, int Y)
        {
            int num = -1;
            int num2 = 0;
            checked
            {
                do
                {
                    if (student_id[num2].Equals(id) &
                        ip_address[num2].Equals(ip))
                    {
                        num = num2;
                    }
                    num2++;
                } while (num2 <= 200);
                if (num >= 0 & num <= 200)
                {
                    inuse[num] = true;
                    active[num] = true;
                    moves[num] = true;
                    speedX[num] = X;
                    speedY[num] = Y;
                    if (command_count[num] < 22)
                    {
                        command_count[num] = command_count[num] + 1;
                    }
                }
            }
        }

        private void addflag(string id, string ip, int new_flag)
        {
            int num = -1;
            int num2 = 0;
            checked
            {
                do
                {
                    if (student_id[num2].Equals(id) &
                        ip_address[num2].Equals(ip))
                    {
                        num = num2;
                    }
                    num2++;
                } while (num2 <= 200);
                if (num >= 0 & num <= 200)
                {
                    inuse[num] = true;
                    active[num] = true;
                    flag[num] = new_flag;
                    if (command_count[num] < 22)
                    {
                        command_count[num] = command_count[num] + 1;
                    }
                }
            }
        }

        private void addServerShip(int index, string name, int X, int Y, int ShipType)
        {
            if (index >= 0 & index <= 200)
            {
                inuse[index] = true;
                active[index] = true;
                student_id[index] = name;
                student_firstname[index] = "XXX";
                student_familyname[index] = "XXX";
                ip_address[index] = "";
                destination_address[index] = "";
                source_address[index] = "";
                message[index] = "";
                ship_type[index] = ShipType % 3;
                shipX[index] = X;
                shipY[index] = Y;
                speedX[index] = 0;
                speedY[index] = 0;
                fire[index] = false;
                fireX[index] = 0;
                fireY[index] = 0;
                health[index] = 10;
            }
        }

        private void updateNastyShips(int index)
        {
            checked
            {
                if (index >= 0 & index < 20)
                {
                    if (speedX[index] == 0 & speedX[index] == 0)
                    {
                        speedX[index] = (int)Math.Round(10f * _random.NextDouble() + 1f) - 5;
                        speedY[index] = (int)Math.Round(10f * _random.NextDouble() + 1f) - 5;
                    }
                    if (shipX[index] > 990)
                    {
                        speedX[index] = -2;
                    }
                    if (shipX[index] < 10)
                    {
                        speedX[index] = 2;
                    }
                    if (shipY[index] > 990)
                    {
                        speedY[index] = -2;
                    }
                    if (shipY[index] < 10)
                    {
                        speedY[index] = 2;
                    }
                    int num = -1;
                    int num2 = 1000;
                    int num3 = 0;
                    do
                    {
                        if (inuse[num3] & active[num3] &&
                            num3 != index &&
                            Distance[index, num3] < VisibleRange(index, num3) &&
                            num2 > Distance[index, num3])
                        {
                            num2 = Distance[index, num3];
                            num = num3;
                        }
                        num3++;
                    } while (num3 <= 200);
                    if (num >= 0 & num <= 200)
                    {
                        if (fireCount[index] < 10)
                        {
                            fire[index] = true;
                            fireX[index] = shipX[num];
                            fireY[index] = shipY[num];
                            fireCount[index] = fireCount[index] + 1;
                        }
                        else
                        {
                            fireCount[index] = 1;
                        }
                        if (shipX[index] < shipX[num])
                        {
                            speedX[index] = 2;
                        }
                        if (shipX[index] > shipX[num])
                        {
                            speedX[index] = -2;
                        }
                        if (shipY[index] < shipY[num])
                        {
                            speedY[index] = 2;
                        }
                        if (shipY[index] > shipY[num])
                        {
                            speedY[index] = -2;
                        }
                        flag[index] = flag[num];
                    }
                    moves[index] = true;
                }
            }
        }

        private void updateServerShips(int index)
        {
            checked
            {
                if (index >= 0 & index <= 200)
                {
                    if (speedX[index] == 0 & speedX[index] == 0)
                    {
                        speedX[index] = (int)Math.Round(10f * _random.NextDouble() + 1f) - 5;
                        speedY[index] = (int)Math.Round(10f * _random.NextDouble() + 1f) - 5;
                    }
                    if (shipX[index] > 990)
                    {
                        speedX[index] = -2;
                    }
                    if (shipX[index] < 10)
                    {
                        speedX[index] = 2;
                    }
                    if (shipY[index] > 990)
                    {
                        speedY[index] = -2;
                    }
                    if (shipY[index] < 10)
                    {
                        speedY[index] = 2;
                    }
                    switch (index)
                    {
                        case 0:
                            {
                                int num = 0;
                                int num2 = -1;
                                int num3 = 1000;
                                int num4 = 0;
                                do
                                {
                                    if (inuse[num4] & active[num4])
                                    {
                                        if ((num4 != 0 & ((shipX[0] < 500 & num4 != 4 & num4 != 5 & num4 != 6) |
                                                          shipX[0] >= 500)) &&
                                            Distance[0, num4] < VisibleRange(0, num4) &&
                                            num3 > Distance[0, num4])
                                        {
                                            num3 = Distance[0, num4];
                                            num2 = num4;
                                        }
                                        num++;
                                    }
                                    num4++;
                                } while (num4 <= 200);
                                if (num2 >= 0 & num2 <= 200)
                                {
                                    if (fireCount[0] < 10)
                                    {
                                        fire[0] = true;
                                        fireX[0] = shipX[num2];
                                        fireY[0] = shipY[num2];
                                        fireCount[0] = fireCount[index] + 1;
                                    }
                                    else
                                    {
                                        fireCount[0] = 1;
                                    }
                                    if (shipX[0] < shipX[num2])
                                    {
                                        speedX[0] = 2;
                                    }
                                    if (shipX[0] > shipX[num2])
                                    {
                                        speedX[0] = -2;
                                    }
                                    if (shipY[0] < shipY[num2])
                                    {
                                        speedY[0] = 2;
                                    }
                                    if (shipY[0] > shipY[num2])
                                    {
                                        speedY[0] = -2;
                                    }
                                    flag[0] = flag[num2];
                                }
                                moves[0] = true;
                                if (server_type == 0 & num > 15)
                                {
                                    int num5 = 0;
                                    int num6 = 0;
                                    int num7 = 0;
                                    num4 = 7;
                                    do
                                    {
                                        if (ship_type[num4] == 0)
                                        {
                                            num5++;
                                        }
                                        if (ship_type[num4] == 1)
                                        {
                                            num6++;
                                        }
                                        if (ship_type[num4] == 2)
                                        {
                                            num7++;
                                        }
                                        num4++;
                                    } while (num4 <= 200);
                                    if (num5 > num6 + num7)
                                    {
                                        ship_type[0] = 2;
                                    }
                                    if (num6 > num5 + num7)
                                    {
                                        ship_type[0] = 0;
                                    }
                                    if (num7 > num6 + num5)
                                    {
                                        ship_type[0] = 1;
                                    }
                                }
                                break;
                            }
                        case 1:
                            {
                                int num2 = -1;
                                int num3 = 1000;
                                int num4 = 0;
                                do
                                {
                                    if ((inuse[num4] & active[num4]) && num4 != index &&
                                        Distance[index, num4] < VisibleRange(index, num4) &&
                                        num3 > Distance[index, num4])
                                    {
                                        num3 = Distance[index, num4];
                                        num2 = num4;
                                    }
                                    num4++;
                                } while (num4 <= 200);
                                if (num2 >= 0 & num2 <= 200)
                                {
                                    fire[index] = true;
                                    fireX[index] = shipX[num2];
                                    fireY[index] = shipY[num2];
                                    if (Distance[index, num2] < (FiringRange(index) * 2) / 3)
                                    {
                                        if (shipX[index] < shipX[num2])
                                        {
                                            speedX[index] = -2;
                                        }
                                        if (shipX[index] > shipX[num2])
                                        {
                                            speedX[index] = 2;
                                        }
                                        if (shipY[index] < shipY[num2])
                                        {
                                            speedY[index] = -2;
                                        }
                                        if (shipY[index] > shipY[num2])
                                        {
                                            speedY[index] = 2;
                                        }
                                    }
                                }
                                moves[index] = true;
                                return;
                            }
                        case 2:
                            {
                                int num2 = -1;
                                int num3 = 1000;
                                int num4 = 0;
                                do
                                {
                                    if (inuse[num4] & active[num4] &&
                                        num4 != index &&
                                        Distance[index, num4] < VisibleRange(index, num4) &&
                                        num3 > Distance[index, num4])
                                    {
                                        num3 = Distance[index, num4];
                                        num2 = num4;
                                    }
                                    num4++;
                                } while (num4 <= 200);
                                if (num2 >= 0 & num2 <= 200)
                                {
                                    fire[index] = true;
                                    fireX[index] = shipX[num2];
                                    fireY[index] = shipY[num2];
                                }
                                if (speedX[index] == -2)
                                {
                                    speedX[index] = -1;
                                }
                                if (speedY[index] == -2)
                                {
                                    speedY[index] = -1;
                                }
                                moves[index] = true;
                                return;
                            }
                        case 3:
                            moves[index] = true;
                            return;
                        case 4:
                        case 5:
                        case 6:
                            {
                                int num2 = -1;
                                int num3 = 1000;
                                int num4 = 0;
                                do
                                {
                                    if ((inuse[num4] & active[num4]) &&
                                        (num4 != 4 & num4 != 5 & num4 != 6 &
                                         ((shipX[num4] < 500 & num4 != 0) | shipX[num4] >= 500)) &&
                                        Distance[index, num4] < VisibleRange(index, num4) &&
                                        num3 > Distance[index, num4])
                                    {
                                        num3 = Distance[index, num4];
                                        num2 = num4;
                                    }
                                    num4++;
                                } while (num4 <= 200);
                                if (num2 >= 0 & num2 <= 200)
                                {
                                    fire[index] = true;
                                    fireX[index] = shipX[num2];
                                    fireY[index] = shipY[num2];
                                    flag[index] = flag[num2];
                                }
                                int num8 = 0;
                                int num9 = 0;
                                int num10 = 0;
                                num4 = 4;
                                do
                                {
                                    if (num10 < Distance[index, num4])
                                    {
                                        num10 = Distance[index, num4];
                                    }
                                    num8 += shipX[num4];
                                    num9 += shipY[num4];
                                    num4++;
                                } while (num4 <= 6);
                                num8 = (int)Math.Round(num8 / 3.0);
                                num9 = (int)Math.Round(num9 / 3.0);
                                if (num10 > 10)
                                {
                                    if (shipX[index] < num8)
                                    {
                                        speedX[index] = 2;
                                    }
                                    if (shipX[index] > num8)
                                    {
                                        speedX[index] = -2;
                                    }
                                    if (shipY[index] < num9)
                                    {
                                        speedY[index] = 2;
                                    }
                                    if (shipY[index] > num9)
                                    {
                                        speedY[index] = -2;
                                    }
                                }
                                else
                                {
                                    if (num2 >= 0 & num2 <= 200)
                                    {
                                        if (shipX[index] < shipX[num2])
                                        {
                                            speedX[index] = 2;
                                        }
                                        if (shipX[index] > shipX[num2])
                                        {
                                            speedX[index] = -2;
                                        }
                                        if (shipY[index] < shipY[num2])
                                        {
                                            speedY[index] = 2;
                                        }
                                        if (shipY[index] > shipY[num2])
                                        {
                                            speedY[index] = -2;
                                        }
                                    }
                                    else
                                    {
                                        if (shipX[index] < packX + index)
                                        {
                                            speedX[index] = 2;
                                        }
                                        if (shipX[index] > packX + index)
                                        {
                                            speedX[index] = -2;
                                        }
                                        if (shipY[index] < packY + index)
                                        {
                                            speedY[index] = 2;
                                        }
                                        if (shipY[index] > packY + index)
                                        {
                                            speedY[index] = -2;
                                        }
                                    }
                                    num4 = 4;
                                    do
                                    {
                                        if (num4 != index && health[num4] < 4)
                                        {
                                            fire[index] = true;
                                            fireX[index] = shipX[num4];
                                            fireY[index] = shipY[num4];
                                        }
                                        num4++;
                                    } while (num4 <= 6);
                                }
                                moves[index] = true;
                                break;
                            }
                        default:
                            return;
                    }
                }
            }
        }

        private void resize_form()
        {
            int height = Height;
            int width = Width;
            checked
            {
                int num = height - 70;
                if (num < 20)
                {
                    num = 20;
                }
                int num2 = width - 200;
                if (num2 < 20)
                {
                    num2 = 20;
                }
                BattleArea.Left = 20;
                BattleArea.Top = 20;
                BattleArea.Height = num;
                BattleArea.Width = num2;
                int num3 = height - 100;
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
                btnRestore.Left = Scoreboard.Left + 107;
                btnRestore.Top = Scoreboard.Top + num3 + 10;
            }
        }

        private int Next_Position()
        {
            return (int)Math.Round(1000f * _random.NextDouble() + 1f);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string str = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0].ToString();
            Text = "Battleships Server (" + str + ")";
            BattleArea.BorderStyle = BorderStyle.Fixed3D;
            displayNames = false;
            int num = 0;
            checked
            {
                do
                {
                    int num2 = 0;
                    do
                    {
                        Area[num, num2] = -1000;
                        num2++;
                    } while (num2 <= 1000);
                    num++;
                } while (num <= 1000);
                num = 0;
                do
                {
                    int num2 = 0;
                    do
                    {
                        Distance[num, num2] = 10000;
                        num2++;
                    } while (num2 <= 200);
                    num++;
                } while (num <= 200);
                resize_form();
                initialise_ships();
                switch (server_type)
                {
                    case 0:
                        addServerShip(0, "Marks_24", Next_Position(), Next_Position(), 0);
                        addServerShip(1, "Hit_Run_", Next_Position(), Next_Position(), 2);
                        addServerShip(2, "Rand_Hit", Next_Position(), Next_Position(), 1);
                        addServerShip(3, "Marks_00", Next_Position(), Next_Position(), 0);
                        addServerShip(4, "Pack_01", Next_Position(), Next_Position(), 0);
                        addServerShip(5, "Pack_02", Next_Position(), Next_Position(), 0);
                        addServerShip(6, "Pack_03", Next_Position(), Next_Position(), 1);
                        break;
                    case 1:
                        num = 0;
                        do
                        {
                            addServerShip(num, "Nasty_" + num.ToString().Trim(),
                                Next_Position(),
                                Next_Position(),
                                num % 3);
                            num++;
                        } while (num <= 19);
                        break;
                }
                Scoreboard.Text = "";
                receivingUdpClient = new UdpClient(1924);
                ThreadReceive = new Thread(ReceiveMessages);
                ThreadReceive.Start();
                OneSecondTimer.Enabled = true;
            }
        }

        private void ReceiveMessages()
        {
            try
            {
                //                ProjectData.ClearProjectError();
                checked
                {
                    while (true)
                    {
                        byte[] bytes = receivingUdpClient.Receive(ref RemoteIpEndPoint);
                        string ip = RemoteIpEndPoint.Address.ToString();
                        string @string = Encoding.ASCII.GetString(bytes);
                        int num3;
                        if (@string.Length - 1 > 150)
                        {
                            illegalMessage(ip);
                            num3 = 150;
                        }
                        else
                        {
                            num3 = @string.Length - 1;
                        }
                        if (@string.StartsWith("Register"))
                        {
                            int num4 = 0;
                            string text = "";
                            string text2 = "";
                            string text3 = "";
                            string text4 = "";

                            int num5 = num3;
                            for (int i = 10; i <= num5; i++)
                            {
                                string text5 = @string.Substring(i, 1);
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

                            addShip(text, text2, text3, ip,
                                Next_Position(),
                                Next_Position(),
                                Convert.ToInt32(text4.Trim()));
                        }
                        else
                        {
                            if (@string.StartsWith("Fire"))
                            {
                                int num4 = 0;
                                string text = "";
                                string text6 = "";
                                string text7 = "";
                                int num6 = num3;
                                for (int i = 5; i <= num6; i++)
                                {
                                    string text5 = @string.Substring(i, 1);
                                    if (text5.Equals(","))
                                    {
                                        num4++;
                                    }
                                    else
                                    {
                                        if (num4 != 0)
                                        {
                                            if (num4 != 1)
                                            {
                                                text7 += text5;
                                            }
                                            else
                                            {
                                                text6 += text5;
                                            }
                                        }
                                        else
                                        {
                                            text += text5;
                                        }
                                    }
                                }
                                if (int.Parse(text6) >= 0 & int.Parse(text6) <= 1000 &
                                    int.Parse(text7) >= 0 & int.Parse(text7) <= 1000)
                                {
                                    addFire(text, ip, int.Parse(text6), int.Parse(text7));
                                }
                            }
                            else
                            {
                                if (@string.StartsWith("Move"))
                                {
                                    int num4 = 0;
                                    string text = "";
                                    string text6 = "";
                                    string text7 = "";
                                    int num7 = num3;
                                    for (int i = 5; i <= num7; i++)
                                    {
                                        string text5 = @string.Substring(i, 1);
                                        if (text5.Equals(","))
                                        {
                                            num4++;
                                        }
                                        else
                                        {
                                            if (num4 != 0)
                                            {
                                                if (num4 != 1)
                                                {
                                                    text7 += text5;
                                                }
                                                else
                                                {
                                                    text6 += text5;
                                                }
                                            }
                                            else
                                            {
                                                text += text5;
                                            }
                                        }
                                    }
                                    if (int.Parse(text6) >= -2 & int.Parse(text6) <= 2 &
                                        int.Parse(text7) >= -2 & int.Parse(text7) <= 2)
                                    {
                                        addmoves(text, ip, int.Parse(text6),
                                            int.Parse(text7));
                                    }
                                }
                                else
                                {
                                    if (@string.StartsWith("Message"))
                                    {
                                        int num4 = 0;
                                        string text = "";
                                        string text8 = "";
                                        string text9 = "";
                                        string text10 = "";
                                        int num8 = num3;
                                        for (int i = 8; i <= num8; i++)
                                        {
                                            string text5 = @string.Substring(i, 1);
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
                                        addMessage(text, ip, text8, text9, text10);
                                    }
                                    else
                                    {
                                        if (@string.StartsWith("Flag"))
                                        {
                                            int num4 = 0;
                                            string text = "";
                                            string text11 = "";
                                            int num9 = num3;
                                            for (int i = 5; i <= num9; i++)
                                            {
                                                string text5 = @string.Substring(i, 1);
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
                                            addflag(text, ip, int.Parse(text11));
                                        }
                                        else
                                        {
                                            illegalMessage(ip);
                                        }
                                    }
                                }
                            }
                        }
                        Application.DoEvents();
                    }
                }
            }
            finally
            {
            }
        }

        private void illegalMessage(string ip)
        {
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            resize_form();
        }

        private void BattleArea_Click(object sender, EventArgs e)
        {
            displayNames ^= true;
        }

        private void OneSecondTimer_Tick(object sender, EventArgs e)
        {
            int num2;
            int num20;
            try
            {
                Image image = new Bitmap(BattleArea.Width, BattleArea.Height);
                Graphics graphics = Graphics.FromImage(image);
                UdpClient udpClient = new UdpClient();
                byte[] array = new byte[0];
                //                ProjectData.ClearProjectError();
                OneSecondTimer.Enabled = false;
                checked
                {
                    packTimer--;
                    if (packTimer > 0)
                    {
                        goto IL_104;
                    }
                    packX = (int)Math.Round((double)(800f * _random.NextDouble() + 100f));
                    if (packX <= 900)
                    {
                        goto IL_B2;
                    }
                    packX = 900;
                    IL_B2:
                    packY = (int)Math.Round((double)(800f * _random.NextDouble() + 100f));
                    if (packY <= 900)
                    {
                        goto IL_F6;
                    }
                    packY = 900;
                    IL_F6:
                    packTimer = 180;
                    IL_104:
                    int i = 0;
                    do
                    {
                        new_health[i] = health[i];
                        Application.DoEvents();
                        i++;
                    } while (i <= 200);
                    i = 0;
                    do
                    {
                        if (inuse[i] & active[i] & fire[i])
                        {
                            int j = 0;
                            do
                            {
                                if (inuse[j] & active[j])
                                {
                                    if (i != j)
                                    {
                                        if (Distance[i, j] <= FiringRange(i))
                                        {
                                            if (fireX[i] == shipX[j] & fireY[i] == shipY[j])
                                            {
                                                if (Distance[i, j] <
                                                    ((((FiringRange(i) * health[i]) * _random.NextDouble()) / 8) + 1))
                                                {
                                                    if (score[i] < 32000)
                                                    {
                                                        score[i] += Damage(i, j);
                                                    }
                                                    if (score[j] > -32000)
                                                    {
                                                        score[j] -= Damage(i, j);
                                                    }
                                                    new_health[j] -= Damage(i, j);
                                                }
                                            }
                                        }
                                    }
                                }
                                j++;
                            } while (j <= 200);
                        }
                        fire[i] = false;
                        Application.DoEvents();
                        i++;
                    } while (i <= 200);
                    i = 0;
                    do
                    {
                        health[i] = new_health[i];
                        if (inuse[i] & active[i])
                        {
                            if (health[i] <= 0)
                            {
                                shipX[i] = Next_Position();
                                shipY[i] = Next_Position();
                                speedX[i] = (int)Math.Round((double)(5f * _random.NextDouble() - 2f));
                                speedY[i] = (int)Math.Round((double)(5f * _random.NextDouble() - 2f));
                                health[i] = 10;
                            }
                        }
                        Application.DoEvents();
                        i++;
                    } while (i <= 200);
                    i = 0;
                    do
                    {
                        if (inuse[i] & active[i] & moves[i])
                        {
                            if (speedX[i] > 2)
                            {
                                speedX[i] = 2;
                            }
                            if (speedX[i] < -2)
                            {
                                speedX[i] = -2;
                            }
                            if (speedY[i] > 2)
                            {
                                speedY[i] = 2;
                            }
                            if (speedY[i] < -2)
                            {
                                speedY[i] = -2;
                            }
                            float num3 = (float)(health[i] * speedX[i] / 10.0);
                            float num4 = (float)(health[i] * speedY[i] / 10.0);
                            shipX[i] = (int)Math.Round(shipX[i] + num3);
                            shipY[i] = (int)Math.Round(shipY[i] + num4);
                            if (shipX[i] < 2)
                            {
                                shipX[i] = 2;
                            }
                            if (shipX[i] > 995)
                            {
                                shipX[i] = 995;
                            }
                            if (shipY[i] < 5)
                            {
                                shipY[i] = 5;
                            }
                            if (shipY[i] > 995)
                            {
                                shipY[i] = 995;
                            }
                        }
                        moves[i] = false;
                        Application.DoEvents();
                        i++;
                    } while (i <= 200);
                    Font font = new Font("Verdana", 8f);
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    graphics.FillRegion(Brushes.LightCyan, new Region(BattleArea.ClientRectangle));
                    i = 0;
                    do
                    {
                        float num5;
                        float num6;
                        unchecked
                        {
                            num5 = (float)(checked((BattleArea.Width - 5) * shipX[i]) / 1000.0 -
                                            2.0);
                            num6 = (float)(BattleArea.Height -
                                            checked(BattleArea.Height * shipY[i]) / 1000.0 - 2.0);
                        }
                        if (inuse[i] & active[i])
                        {
                            switch (ship_type[i])
                            {
                                case 0:
                                    switch (health[i])
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
                                case 1:
                                    switch (health[i])
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
                                case 2:
                                    switch (health[i])
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
                            if (displayNames)
                            {
                                Point p = new Point
                                {
                                    X = (int)Math.Round(num5 + 5f),
                                    Y = (int)Math.Round(num6)
                                };
                                graphics.DrawString(student_id[i], font, Brushes.DarkGreen, p);
                            }
                        }
                        Application.DoEvents();
                        i++;
                    } while (i <= 200);
                    BattleArea.Image = image;
                    graphics.Dispose();
                    string text = "";
                    int num7 = 0;
                    Set_Marks();
                    i = 0;
                    do
                    {
                        if (inuse[i] & active[i])
                        {
                            display_id[num7] = student_id[i];
                            if (show_marks)
                            {
                                display_score[num7] = marks[i];
                            }
                            else
                            {
                                display_score[num7] = score[i];
                            }
                            num7++;
                        }
                        i++;
                    } while (i <= 200);
                    int num8 = num7 - 1;
                    for (i = 0; i <= num8; i++)
                    {
                        int num9 = num7 - 1;
                        for (int j = 0; j <= num9; j++)
                        {
                            if (display_score[i] > display_score[j])
                            {
                                string text2 = display_id[i];
                                int num10 = display_score[i];
                                display_id[i] = display_id[j];
                                display_score[i] = display_score[j];
                                display_id[j] = text2;
                                display_score[j] = num10;
                            }
                        }
                    }
                    int num11 = num7 - 1;
                    for (i = 0; i <= num11; i++)
                    {
                        text = string.Concat(text, display_id[i], " ", display_score[i].ToString(), "\r\n");
                    }
                    Scoreboard.Text = text;
                    i = 0;
                    do
                    {
                        int j = 0;
                        do
                        {
                            double num12 = shipX[i] - shipX[j];
                            double num13 = shipY[i] - shipY[j];
                            double expr_D56 = num12;
                            double d;
                            unchecked
                            {
                                double arg_D5C_0 = expr_D56 * expr_D56;
                                double expr_D5A = num13;
                                d = arg_D5C_0 + expr_D5A * expr_D5A;
                            }
                            Distance[i, j] = (int)Math.Round(Math.Sqrt(d));
                            j++;
                        } while (j <= 200);
                        Application.DoEvents();
                        i++;
                    } while (i <= 200);
                    switch (server_type)
                    {
                        case 0:
                            updateServerShips(0);
                            updateServerShips(1);
                            updateServerShips(2);
                            updateServerShips(3);
                            updateServerShips(4);
                            updateServerShips(5);
                            updateServerShips(6);
                            break;
                        case 1:
                            i = 0;
                            do
                            {
                                updateNastyShips(i);
                                i++;
                            } while (i <= 19);
                            break;
                    }
                    int num14 = 0;
                    switch (server_type)
                    {
                        case 0:
                            num14 = 7;
                            break;
                        case 1:
                            num14 = 20;
                            break;
                        case 2:
                            num14 = 0;
                            break;
                    }
                    for (i = num14; i <= 200; i++)
                    {
                        if (inuse[i] & active[i])
                        {
                            string text3 = string.Concat(shipX[i].ToString(), ",", shipY[i].ToString(), ",",
                                health[i].ToString(), ",", flag[i].ToString());
                            number_of_ships = 0;
                            int j = 0;
                            do
                            {
                                if (((inuse[j] & active[j] & i != j) &&
                                     (Distance[i, j] <= VisibleRange(i, j))))
                                {
                                    list_of_ships[number_of_ships] = j;
                                    number_of_ships++;
                                }
                                j++;
                            } while (j <= 200);
                            if (number_of_ships > 1)
                            {
                                int num15 = number_of_ships - 1;
                                for (j = 0; j <= num15; j++)
                                {
                                    int num16 = (int)Math.Round((float)number_of_ships * _random.NextDouble());
                                    int num17 = (int)Math.Round((float)number_of_ships * _random.NextDouble() + 1f);
                                    if (num16 < number_of_ships & num17 < number_of_ships)
                                    {
                                        int num18 = list_of_ships[num16];
                                        list_of_ships[num16] = list_of_ships[num17];
                                        list_of_ships[num17] = num18;
                                    }
                                }
                            }
                            if (number_of_ships > 0)
                            {
                                int num19 = number_of_ships - 1;
                                for (j = 0; j <= num19; j++)
                                {
                                    text3 = string.Concat(text3, "|",
                                        shipX[list_of_ships[j]].ToString(), ",",
                                        shipY[list_of_ships[j]].ToString(), ",",
                                        health[list_of_ships[j]].ToString(), ",",
                                        flag[list_of_ships[j]].ToString(), ",",
                                        ship_type[list_of_ships[j]].ToString());
                                }
                            }
                            text3 += "\0";
                            IPAddress addr = IPAddress.Parse(ip_address[i]);
                            int port = 1925;
                            udpClient.Connect(addr, port);
                            array = Encoding.ASCII.GetBytes(text3);
                            UdpClient arg_1269_0 = udpClient;
                            byte[] expr_1266 = array;
                            arg_1269_0.Send(expr_1266, expr_1266.Length);
                            Application.DoEvents();
                            if (!destination_address[i].Equals("") &
                                !message[i].Equals("") &
                                !getIP(destination_address[i]).Equals("") &
                                destination_address[i].Length > 4)
                            {
                                string s = string.Concat("Message ", destination_address[i], ", ", source_address[i],
                                    ", ", message[i], "\0");
                                addr = IPAddress.Parse(getIP(destination_address[i]));
                                port = 1925;
                                udpClient.Connect(addr, port);
                                array = Encoding.ASCII.GetBytes(s);
                                UdpClient arg_1396_0 = udpClient;
                                byte[] expr_1393 = array;
                                arg_1396_0.Send(expr_1393, expr_1393.Length);
                                destination_address[i] = "";
                                message[i] = "";
                            }
                            Application.DoEvents();
                        }
                    }
                    udpClient.Close();
                    TenMinuteCounter += 1L;
                    if (TenMinuteCounter < 6000L)
                    {
                        goto IL_144A;
                    }
                    if (!(false & server_type == 0))
                    {
                        goto IL_143C;
                    }
                    Print_Marks();
                    IL_143C:
                    TenMinuteCounter = 0L;
                    IL_144A:
                    i = 0;
                    do
                    {
                        if (command_count[i] > 20)
                        {
                            illegalMessage(ip_address[i]);
                        }
                        command_count[i] = 0;
                        i++;
                    } while (i <= 200);
                    OneSecondTimer.Interval = 95;
                    OneSecondTimer.Enabled = true;
                }
            }
            finally
            {
            }
        }

        private void Set_Marks()
        {
            int num = score[3];
            int num2 = 4;
            checked
            {
                do
                {
                    if ((inuse[num2] & active[num2]) && num < score[num2])
                    {
                        num = score[num2];
                    }
                    num2++;
                } while (num2 <= 200);
                int num3 = score[0];
                int num4 = score[3];
                num2 = 0;
                do
                {
                    if (inuse[num2] & active[num2])
                    {
                        if (score[num2] <= num3)
                        {
                            if (num3 - num4 > 0)
                            {
                                marks[num2] =
                                    (int)Math.Round(24 * (score[num2] - num4) /
                                                     (double)(num3 - num4));
                            }
                            else
                            {
                                marks[num2] = 0;
                            }
                            if (marks[num2] > 24)
                            {
                                marks[num2] = 24;
                            }
                            if (marks[num2] < 0)
                            {
                                marks[num2] = 0;
                            }
                        }
                        else
                        {
                            if (num - num3 > 0)
                            {
                                marks[num2] =
                                    (int)Math.Round(
                                        checked(36 * (score[num2] - num3) /
                                                (double)(num - num3)) + 24.0);
                            }
                            else
                            {
                                marks[num2] = 0;
                            }
                            if (marks[num2] > 60)
                            {
                                marks[num2] = 60;
                            }
                            if (marks[num2] < 25)
                            {
                                marks[num2] = 25;
                            }
                        }
                    }
                    num2++;
                } while (num2 <= 200);
            }
        }

        private void Print_Marks()
        {
            StreamWriter streamWriter = new StreamWriter(DateTime.Now.ToString("ddd_HH_mm_MMM_d") + "_Attendance.txt");
            Set_Marks();
            int num = 0;
            checked
            {
                do
                {
                    if (inuse[num] & active[num])
                    {
                        string value = string.Concat(student_id[num], ",  ",
                            student_familyname[num], ",   ",
                            student_firstname[num], ",   ",
                            ip_address[num], ",   ",
                            score[num].ToString(), ",   ",
                            marks[num].ToString(), ",   ",
                            ship_type[num].ToString());
                        streamWriter.WriteLine(value);
                    }
                    num++;
                } while (num <= 200);
                streamWriter.Close();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (false & server_type == 0)
            {
                Print_Marks();
            }
            ThreadReceive.Abort();
            receivingUdpClient.Close();
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            int num2;
            int num6;
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                OneSecondTimer.Enabled = false;
                initialise_ships();
                number_of_ships = 0;
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;
                if (openFileDialog.ShowDialog() != DialogResult.OK)
                {
                }
                string fileName = openFileDialog.FileName;
                checked
                {
                    using (StreamReader streamReader = new StreamReader(fileName, true))
                    {
                        int num3 = 0;
                        string text = streamReader.ReadLine();
                        while (text != null)
                        {
                            if (text.Length > 5)
                            {
                                int num4 = 0;
                                string text2 = "";
                                string text3 = "";
                                string text4 = "";
                                string text5 = "";
                                string text6 = "";
                                string text7 = "";
                                string text8 = "";
                                int num5 = text.Length - 1;
                                for (int i = 0; i <= num5; i++)
                                {
                                    string text9 = text.Substring(i, 1);
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
                                if (num3 >= 0 & num3 <= 200)
                                {
                                    inuse[num3] = true;
                                    active[num3] = true;
                                    student_id[num3] = text2.Trim();
                                    student_firstname[num3] = text3.Trim();
                                    student_familyname[num3] = text4.Trim();
                                    ip_address[num3] = text5.Trim();
                                    destination_address[num3] = "";
                                    source_address[num3] = "";
                                    message[num3] = "";
                                    shipX[num3] =
                                        Next_Position();
                                    shipY[num3] =
                                        Next_Position();
                                    moves[num3] = false;
                                    speedX[num3] = 0;
                                    speedY[num3] = 0;
                                    fire[num3] = false;
                                    fireX[num3] = 0;
                                    fireY[num3] = 0;
                                    health[num3] = 10;
                                    score[num3] = Convert.ToInt32(text6.Trim());
                                    marks[num3] = Convert.ToInt32(text7.Trim());
                                    ship_type[num3] = Convert.ToInt32(text8.Trim());
                                }
                                text = streamReader.ReadLine();
                                num3++;
                            }
                        }
                        number_of_ships = num3;
                        if (streamReader != null)
                        {
                            streamReader.Close();
                        }
                    }
                    OneSecondTimer.Enabled = true;
                }
            }
            finally
            {
            }
        }

        private void BattleArea_DoubleClick(object sender, EventArgs e)
        {
            show_marks ^= true;
            displayNames ^= true;
        }

        [DebuggerNonUserCode]
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && components != null)
                {
                    components.Dispose();
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
            btnRestore = new Button();
            getFile = new OpenFileDialog();
            ((ISupportInitialize)BattleArea).BeginInit();
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
            btnRestore.Location = new Point(547, 421);
            btnRestore.Name = "btnRestore";
            btnRestore.Size = new Size(33, 13);
            btnRestore.TabIndex = 3;
            btnRestore.UseVisualStyleBackColor = true;
            getFile.FileName = "OpenFileDialog1";
            AutoScaleDimensions = new SizeF(6f, 13f);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(596, 458);
            Controls.Add(btnRestore);
            Controls.Add(Status);
            Controls.Add(Scoreboard);
            Controls.Add(BattleArea);
            Name = "Form1";
            Text = "Battleships";
            ((ISupportInitialize)BattleArea).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}