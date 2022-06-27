using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Swiper
{
    public partial class Form1 : Form
    {
        Bitmap off;
        Bitmap on;
        Timer t = new Timer();

        class MainCard
        {
            public int x;
            public int y;
            public int w;
            public int h;
            public Color clr;
            public Color textClr;
            public String text;
            public String status;
            public bool redIsActive;
            public bool blueIsActive;
            public List<RedCard> redCards = new List<RedCard>();
            public List<blueCards> blueCards = new List<blueCards>();
            public List<GreenCard> greenCards = new List<GreenCard>();
        }

        class RedCard
        {
            public int x;
            public int y;
            public int w;
            public int h;
            public Color clr;
            public String text;
        }

        class blueCards
        {
            public int x;
            public int textX;  ///////
            public int y;
            public int w;
            public int h;
            public Color clr;
            public String text;
        }

        class GreenCard
        {
            public int x;
            public int y;
            public int w;
            public int h;
            public Color clr;
            public String text;
        }

        List<MainCard> mainCards = new List<MainCard>();
       
        int noOfCards = 20;
        int vY = 0;
        int iWhich = -1;
        int prevX, prevY;
        int ctTimer = 0;
        int slideSpeed = 100;
        int pointsY1 = 0;
        int pointsY2 = 0;
        int iWhichDelete = -1;
        int iWhichChange = -1;
        int iTemp = 0;
        int orderedNumber = 0;
        int savedMainCardX = 0;
        int savedColorCardX = 0;
        int savedMainCardW = 0;
        int savedColorCardW = 0;
        int noOfScrolledCards = 0;
        bool cardDrag = false;
        bool redStucked = false;
        bool blueStucked = false;
        bool scrollDrag = false;
        bool accelerateScrollOnY = false;
        bool deleteProgress1 = false;
        bool deleteProgress2 = false;
        bool reorderTheList = false;
        bool deleteProgress2Ready = false;
        bool changeStatusReady = false;
        bool clickToChangeStatusReady = false;
        bool okChangeTheStatus = false;

        public Form1()
        {
            this.MouseDown += Form1_MouseDown;
            this.MouseMove += Form1_MouseMove;
            this.MouseUp += Form1_MouseUp;
            this.Paint += Form1_Paint;
            t.Tick += T_Tick;
            t.Start();
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            off = new Bitmap(this.Width, this.Height);
            on = new Bitmap(this.Width, this.Height);
            createMainCards();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            drawDouble(e.Graphics);
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            scrollDrag = true;
            accelerateScrollOnY = false;
            pointsY1 = 0;
            pointsY2 = 0;

            prevY = e.Y;

            for (int i = 0; i < mainCards.Count; i++)
            {
                if (e.X > mainCards[i].x && e.X < mainCards[i].x + mainCards[i].w &&
                    e.Y > mainCards[i].y - vY && e.Y < mainCards[i].y - vY + mainCards[i].h)
                {
                    iWhich = i;
                    prevX = e.X;
                    cardDrag = true;
                    break;
                }
            }

            if (true)
            {
                for (int i = 0; i < mainCards.Count; i++)
                {
                    for (int j = 0; j < mainCards[i].redCards.Count; j++)
                    {
                        if (e.X > mainCards[i].redCards[j].x && e.X < mainCards[i].redCards[j].x + mainCards[i].redCards[j].w &&
                            e.Y > mainCards[i].redCards[j].y - vY && e.Y < mainCards[i].redCards[j].y - vY + mainCards[i].redCards[j].h)
                        {
                            iWhichDelete = i;
                            deleteProgress1 = true;
                            break;
                        }
                    }
                }
            }

            if (true)
            {
                for (int i = 0; i < mainCards.Count; i++)
                {
                    for (int j = 0; j < mainCards[i].blueCards.Count; j++)
                    {
                        if (e.X > mainCards[i].blueCards[j].x && e.X < mainCards[i].blueCards[j].x + mainCards[i].blueCards[j].w &&
                            e.Y > mainCards[i].blueCards[j].y - vY && e.Y < mainCards[i].blueCards[j].y - vY + mainCards[i].blueCards[j].h)
                        {
                            iWhichChange = i;
                            clickToChangeStatusReady = true;
                            break;
                        }
                    }
                }
            }

        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (cardDrag)
            {
                redStucked = false;
                blueStucked = false;
                int dx = prevX - e.X;
                if (dx < 0)
                {
                    dx *= -1;
                }

                if ((e.X < prevX || mainCards[iWhich].redIsActive) && !mainCards[iWhich].blueIsActive)
                {
                    scrollDrag = false;
                    mainCards[iWhich].redIsActive = true;
                    if (e.X < prevX)
                    {
                        
                        if (mainCards[iWhich].redCards[0].x >= -5)
                        {
                            mainCards[iWhich].x -= dx;
                            mainCards[iWhich].redCards[0].x -= (dx / 2);
                            mainCards[iWhich].redCards[0].w += (dx / 2);
                            mainCards[iWhich].greenCards[0].x -= (dx);
                            mainCards[iWhich].greenCards[0].w += (dx / 2);
                            prevX = e.X;
                        }

                        if (mainCards[iWhich].greenCards[0].x < (this.Width / 2) - 100 && !deleteProgress2Ready)
                        {
                            savedMainCardX = mainCards[iWhich].x;
                            savedColorCardX = mainCards[iWhich].redCards[0].x;
                            savedMainCardW = mainCards[iWhich].w;
                            savedColorCardW = mainCards[iWhich].redCards[0].w;

                            mainCards[iWhich].x -= dx;
                            mainCards[iWhich].redCards[0].x = 0;
                            mainCards[iWhich].redCards[0].w = this.Width;
                            deleteProgress2Ready = true;
                        }


                    }

                    if (e.X > prevX)
                    {
                        if (mainCards[iWhich].redCards[0].w > 0 && !deleteProgress2Ready)
                        {
                            mainCards[iWhich].w += dx;
                            mainCards[iWhich].x += dx * 2;
                            mainCards[iWhich].redCards[0].x += (dx / 2);
                            mainCards[iWhich].redCards[0].w -= (dx / 2);
                            mainCards[iWhich].greenCards[0].x += dx;
                            mainCards[iWhich].greenCards[0].w -= (dx / 2);
                            prevX = e.X;

                        }
                        if (mainCards[iWhich].redCards[0].x < (this.Width / 2) - 100)
                        {
                            mainCards[iWhich].x = savedMainCardX;
                            mainCards[iWhich].redCards[0].x = savedColorCardX;
                            
                            mainCards[iWhich].w = savedMainCardW;
                            mainCards[iWhich].greenCards[0].x = mainCards[iWhich].w + mainCards[iWhich].x;
                            mainCards[iWhich].redCards[0].w = savedColorCardW;
                            deleteProgress2Ready = false;
                        }

                        fixRedBugs();
                    }
                }

                if ((e.X > prevX || mainCards[iWhich].blueIsActive) && !mainCards[iWhich].redIsActive)
                {
                    scrollDrag = false;
                    mainCards[iWhich].blueIsActive = true;
                    if (e.X < prevX)
                    {
                        if (mainCards[iWhich].x > 0)
                        {
                            mainCards[iWhich].x -= dx;
                            mainCards[iWhich].w += dx;
                            mainCards[iWhich].blueCards[0].w -= dx;
                            prevX = e.X;
                        }
                        if (mainCards[iWhich].blueCards[0].w > (this.Width / 2))
                        {
                            mainCards[iWhich].x = savedMainCardX;
                            mainCards[iWhich].w = savedMainCardW;
                            mainCards[iWhich].blueCards[0].w = savedColorCardW;
                            changeStatusReady = false;
                        }
                    }

                    if (e.X > prevX)
                    {
                        if (mainCards[iWhich].x < this.Width)
                        {
                            mainCards[iWhich].x += dx;
                            mainCards[iWhich].w -= dx;
                            mainCards[iWhich].blueCards[0].w += dx;
                            prevX = e.X;
                        }
                        if (mainCards[iWhich].blueCards[0].w > (this.Width / 2) && !changeStatusReady)
                        {
                            savedMainCardX = mainCards[iWhich].x;
                            savedColorCardX = mainCards[iWhich].blueCards[0].x;
                            savedMainCardW = mainCards[iWhich].w;
                            savedColorCardW = mainCards[iWhich].blueCards[0].w;

                            mainCards[iWhich].x = this.Width;
                            mainCards[iWhich].blueCards[0].w = this.Width;
                            changeStatusReady = true;
                        }
                    }

                    fixBlueBugs();
                }

            }

            if (scrollDrag)
            {
                int dy = prevY - e.Y;
                cardDrag = false;
                if (dy < 0)
                {
                    dy *= -1;
                }

                if (e.Y < prevY && vY < (this.Height * 3) - 300)
                {
                    vY += dy;
                    pointsY1 = dy * 2;
                    prevY = e.Y;
                }

                if (e.Y > prevY && vY > 0)
                {
                    vY -= dy;
                    pointsY2 = dy * 2;
                    prevY = e.Y;
                }
            }

        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            cardDrag = false;
            if (scrollDrag)
            {
                scrollDrag = false;
                accelerateScrollOnY = true;
            }

            if (deleteProgress2Ready)
            {
                deleteProgress2 = true;
                deleteProgress2Ready = false;
                iWhichDelete = iWhich;
                iTemp = iWhichDelete;
            }

            if (changeStatusReady)
            {
                clickToChangeStatusReady = true;
                changeStatusReady = false;
                iWhichChange = iWhich;
            }

            if (mainCards[iWhich].redCards[0].w <= 0 && mainCards[iWhich].redIsActive)
            {
                mainCards[iWhich].redIsActive = false;
            }
            else if(mainCards[iWhich].blueCards[0].w <= 0 && mainCards[iWhich].blueIsActive)
            {
                mainCards[iWhich].blueIsActive = false;
            }
        }

        private void T_Tick(object sender, EventArgs e)
        {
            if (mainCards.Count > 0 && iWhich != -1 && !deleteProgress1 && !deleteProgress2 && !okChangeTheStatus)
            {
                if (!cardDrag && mainCards[iWhich].redCards[0].x < this.Width)
                {
                    for (int k = 0; k < slideSpeed; k++)
                    {
                        if (mainCards[iWhich].redCards[0].x > this.Width - 200 && !redStucked)
                        {
                            mainCards[iWhich].w += 1;
                            mainCards[iWhich].x += 2;
                            mainCards[iWhich].redCards[0].x += 1;
                            mainCards[iWhich].redCards[0].w -= 1;
                            mainCards[iWhich].greenCards[0].x += 2;
                            mainCards[iWhich].greenCards[0].w -= 1;
                            mainCards[iWhich].redIsActive = false;
                            fixRedBugs();
                        }
                        else if (mainCards[iWhich].redCards[0].x > this.Width - 600 && mainCards[iWhich].redCards[0].x < this.Width - 200)
                        {
                            mainCards[iWhich].w += 1;
                            mainCards[iWhich].x += 1;
                            mainCards[iWhich].redCards[0].x += 1;
                            mainCards[iWhich].redCards[0].w -= 1;
                            mainCards[iWhich].greenCards[0].x += 2;
                            mainCards[iWhich].greenCards[0].w -= 1;
                            mainCards[iWhich].redIsActive = true;
                            redStucked = true;
                        }
                    }
                }
                else if (!cardDrag && mainCards[iWhich].blueCards[0].w != 0)
                {
                    for (int k = 0; k < slideSpeed; k++)
                    {
                        if (mainCards[iWhich].blueCards[0].w < 200 && !blueStucked)
                        {
                            mainCards[iWhich].x -= 1;
                            mainCards[iWhich].w += 1;
                            mainCards[iWhich].blueCards[0].w -= 1;
                            mainCards[iWhich].blueIsActive = false;
                            fixBlueBugs();
                        }
                        else if (mainCards[iWhich].blueCards[0].w < 600 && mainCards[iWhich].blueCards[0].w > (mainCards[iWhich].status == "Unread" ? 190 : 190))
                        {
                            mainCards[iWhich].x -= 1;
                            mainCards[iWhich].w += 1;
                            mainCards[iWhich].blueCards[0].w -= 1;
                            mainCards[iWhich].blueIsActive = true;
                            blueStucked = true;
                        }
                    }
                }
            }

            if(accelerateScrollOnY && pointsY1 > 0)
            {
                for (int k = 0; k < pointsY1; k++)
                {
                    if (vY < (this.Height * 3) - 270)
                    {
                        vY ++;
                    }
                }
                pointsY1--;

                if (pointsY1 == 0)
                    accelerateScrollOnY = false;
            }
            else if (accelerateScrollOnY && pointsY2 > 0)
            {
                for (int k = 0; k < pointsY2; k++)
                {
                    if (vY > 0)
                    {
                        vY --;
                    }
                }
                pointsY2--;

                if (pointsY2 == 0)
                    accelerateScrollOnY = false;
            }

            if (deleteProgress1)
            {
                for (int k = 0; k < 180; k++)
                {
                    if (mainCards[iWhichDelete].redCards[0].x > 0)
                    {
                        mainCards[iWhichDelete].x -= 1;
                        mainCards[iWhichDelete].redCards[0].x -= 1;
                        mainCards[iWhichDelete].redCards[0].w += 1;
                    }
                    else
                    {
                        deleteProgress1 = false;
                        deleteProgress2 = true;
                    }
                }
            }

            if (deleteProgress2)
            {
                mainCards[iWhichDelete].redCards[0].h -= 80;
                mainCards[iWhichDelete].w = 0;
                if (mainCards[iWhichDelete].redCards[0].h <= 0)
                {
                    deleteProgress2 = false;
                    mainCards.Remove(mainCards[iWhichDelete]);
                    reorderTheList = true;
                    mainCards[iWhichDelete].redIsActive = false;
                }
            }

            if (reorderTheList)
            {
                if (iTemp >= iWhichDelete)
                {
                    for (int k = 0; k < 100; k++)
                    {
                        mainCards[iTemp].y --;
                        mainCards[iTemp].redCards[0].y --;
                        mainCards[iTemp].blueCards[0].y --;
                        mainCards[iTemp].greenCards[0].y--;
                        orderedNumber ++;
                        if (orderedNumber > mainCards[iTemp].h + 4)
                        {
                            orderedNumber = 0;
                            iTemp++;
                            noOfScrolledCards++;
                        }
                        if (iTemp >= mainCards.Count)
                        {
                            iTemp = 0;
                            reorderTheList = false;
                            orderedNumber = 0;
                            break;
                        }
                        if (noOfScrolledCards > 2)
                        {
                            doReorderTheList();
                            reorderTheList = false;
                            iTemp = 0;
                            orderedNumber = 0;
                            noOfScrolledCards = 0;
                            break;
                        }
                    }
                }
                else if(iTemp < iWhichDelete)
                {
                    iTemp++;
                }
            }

            if (clickToChangeStatusReady || okChangeTheStatus)
            {
                okChangeTheStatus = true;
                if (clickToChangeStatusReady)
                {
                    mainCards[iWhichChange].textClr = mainCards[iWhichChange].textClr == Color.Black ? Color.DodgerBlue : Color.Black;
                    clickToChangeStatusReady = false;
                }

                for (int k = 0; k < (mainCards[iWhichChange].blueCards[0].w > 200 ? 1000 : 80); k++)
                {
                    if (mainCards[iWhichChange].blueCards[0].w > 0)
                    {
                        mainCards[iWhichChange].x--;
                        mainCards[iWhichChange].w++;
                        mainCards[iWhichChange].blueCards[0].w--;
                    }
                    else
                    {
                        mainCards[iWhichChange].blueIsActive = false;
                        clickToChangeStatusReady = false;
                        okChangeTheStatus = false;
                        mainCards[iWhichChange].blueCards[0].text = mainCards[iWhichChange].blueCards[0].text == "Unread" ? "Read" : "Unread";
                        mainCards[iWhichChange].status = mainCards[iWhichChange].status == "Read" ? "Unread" : "Read";
                        break;
                    }
                }
            }

            


            ctTimer++;
            drawDouble(this.CreateGraphics());
        }

      
        void createMainCards()
        {
            int ay = 0;
            for (int i = 0; i < noOfCards; i++)
            {
                MainCard pnn = new MainCard();
                pnn.x = 0;
                pnn.y = ay;
                pnn.w = this.Width;
                pnn.h = 150;
                pnn.clr = Color.WhiteSmoke;
                pnn.text = "You've got a mail.";
                pnn.redIsActive = false;
                pnn.blueIsActive = false;
                pnn.textClr = Color.DodgerBlue;
                pnn.status = "Unread";
                mainCards.Add(pnn);
                ay += 155;
            }

            createRedCards();
            createBlueCards();
            createGreenCards();
        }

        void createRedCards()
        {
            int ay = 0;
            for (int i = 0; i < noOfCards; i++)
            {
                RedCard pnn = new RedCard();
                pnn.x = this.Width;
                pnn.y = ay;
                pnn.w = 0;
                pnn.h = 150;
                pnn.clr = Color.IndianRed;
                pnn.text = "Trash";
                mainCards[i].redCards.Add(pnn);
                ay += 155;
            }
        }

        void createBlueCards()
        {
            int ay = 0;
            for (int i = 0; i < noOfCards; i++)
            {
                blueCards pnn = new blueCards();
                pnn.x = 0;
                pnn.y = ay;
                pnn.w = 0;
                pnn.h = 150;
                pnn.clr = Color.DodgerBlue;
                pnn.text = "Read";
                mainCards[i].blueCards.Add(pnn);
                ay += 155;
            }
        }

        void createGreenCards()
        {
            int ay = 0;
            for (int i = 0; i < noOfCards; i++)
            {
                GreenCard pnn = new GreenCard();
                pnn.x = this.Width;
                pnn.y = ay;
                pnn.w = 0;
                pnn.h = 150;
                pnn.clr = Color.SeaGreen;
                pnn.text = "Archive";
                mainCards[i].greenCards.Add(pnn);
                ay += 155;
            }
        }

        void fixRedBugs()
        {
            if (mainCards[iWhich].x > 0)
            {
                mainCards[iWhich].x = 0;
            }
            if (mainCards[iWhich].w > this.Width)
            {
                mainCards[iWhich].w = this.Width;
            }

            if(mainCards[iWhich].greenCards[0].x > this.Width)
            {
                mainCards[iWhich].greenCards[0].x = this.Width;
                mainCards[iWhich].greenCards[0].w = 0;
            }


            if (mainCards[iWhich].redCards[0].x > this.Width)
            {
                mainCards[iWhich].redCards[0].x = this.Width;
            }
            if(mainCards[iWhich].redCards[0].w < 0)
            {
                mainCards[iWhich].redCards[0].w = 0;
                mainCards[iWhich].redIsActive = false;          //////
            }
        }

        void fixBlueBugs()
        {
            if (mainCards[iWhich].x < 0)
            {
                mainCards[iWhich].x = 0;
                mainCards[iWhich].blueCards[0].x = 0;
                mainCards[iWhich].blueCards[0].w = 0;
                mainCards[iWhich].blueIsActive = false;         //////
            }
        }

        void doReorderTheList()
        {
            for(int i = 0; i < mainCards.Count; i++)
            {
                if (i >= iTemp)
                {
                    mainCards[i].y -= mainCards[i].h + 4;
                    mainCards[i].redCards[0].y -= mainCards[i].h + 4;
                    mainCards[i].blueCards[0].y -= mainCards[i].h + 4;
                    mainCards[i].greenCards[0].y -= mainCards[i].h + 4;
                }
            }
        }
        
        void drawScene(Graphics g)
        {
            g.Clear(Color.White);

            for (int i = 0; i < mainCards.Count; i++)
            {
                SolidBrush br1 = new SolidBrush(mainCards[i].clr);
                SolidBrush br12 = new SolidBrush(mainCards[i].textClr);
                g.FillRectangle(br1, mainCards[i].x, mainCards[i].y - vY, mainCards[i].w, mainCards[i].h);
                g.DrawString(mainCards[i].text + " " + i, new Font("Arial", 14, FontStyle.Bold), br12, mainCards[i].x + 10, mainCards[i].y - vY + 50);

                if (!deleteProgress2)
                {
                    for (int j = 0; j < mainCards[i].greenCards.Count; j++)
                    {
                        SolidBrush br3 = new SolidBrush(mainCards[i].greenCards[j].clr);
                        g.FillRectangle(br3, mainCards[i].greenCards[j].x, mainCards[i].greenCards[j].y - vY, mainCards[i].greenCards[j].w, mainCards[i].greenCards[j].h);
                        g.DrawString(mainCards[i].greenCards[j].text, new Font("Arial", 12, FontStyle.Bold), Brushes.White, mainCards[i].greenCards[j].x + 35, mainCards[i].greenCards[j].y - vY + 52);

                    }
                }

                for (int j = 0; j < mainCards[i].redCards.Count; j++)
                {
                    SolidBrush br2 = new SolidBrush(mainCards[i].redCards[j].clr);
                    g.FillRectangle(br2, mainCards[i].redCards[j].x, mainCards[i].redCards[j].y - vY, mainCards[i].redCards[j].w, mainCards[i].redCards[j].h);
                    g.DrawString(mainCards[i].redCards[j].text, new Font("Arial", 12, FontStyle.Bold), Brushes.White, mainCards[i].redCards[j].x + 35, mainCards[i].redCards[j].y - vY + 52);

                }

                for (int j = 0; j < mainCards[i].blueCards.Count; j++)
                {
                    SolidBrush br2 = new SolidBrush(mainCards[i].blueCards[j].clr);
                    g.FillRectangle(br2, mainCards[i].blueCards[j].x, mainCards[i].blueCards[j].y - vY, mainCards[i].blueCards[j].w, mainCards[i].blueCards[j].h);
                    if (mainCards[i].status == "Unread")
                    {
                        if (changeStatusReady)
                        {
                            g.DrawString(mainCards[i].blueCards[j].text, new Font("Arial", 12, FontStyle.Bold), Brushes.White, mainCards[i].blueCards[j].w - 150, mainCards[i].blueCards[j].y - vY + 52);
                        }
                        else
                        {
                            g.DrawString(mainCards[i].blueCards[j].text, new Font("Arial", 12, FontStyle.Bold), Brushes.White, mainCards[i].blueCards[j].w <= 150 ? mainCards[i].blueCards[j].w - 120 : mainCards[i].blueCards[j].x + 40, mainCards[i].blueCards[j].y - vY + 52);
                        }
                    }
                    else
                    {
                        if (changeStatusReady)
                        {
                            g.DrawString(mainCards[i].blueCards[j].text, new Font("Arial", 12, FontStyle.Bold), Brushes.White, mainCards[i].blueCards[j].w - 180, mainCards[i].blueCards[j].y - vY + 52);
                        }
                        else
                        {
                            g.DrawString(mainCards[i].blueCards[j].text, new Font("Arial", 12, FontStyle.Bold), Brushes.White, mainCards[i].blueCards[j].w <= 180 ? mainCards[i].blueCards[j].w - 150 : mainCards[i].blueCards[j].x + 30, mainCards[i].blueCards[j].y - vY + 52);

                        }
                    }
                }
            }
        }

        void drawDouble(Graphics g)
        {
            Graphics g2 = Graphics.FromImage(off);
            drawScene(g2);
            g.DrawImage(off, 0, 0);
        }
    }
}
