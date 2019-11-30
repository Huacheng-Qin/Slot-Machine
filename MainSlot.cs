using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SlotMachine
{
    public partial class Slots : Form
    {
        private int NumQuarter = 8;         //Number of quarters the user has
        System.Timers.Timer Delay;          //Delay timer for sprite changes
        int counter;                        //Counter for timer
        int win;                            //Which prize the user won (0 is no win)
        bool allowbutton = false;           //Whether or not the user is allowed to press the push button
        bool allowstart = true;             //Whether or not the user is allowed to press the start button
        int antifalse = -1;                 //Prevent the program from generating false wins

        //Initialize
        public Slots()
        {
            InitializeComponent();
            //Update starting money
            UpdateMoney();
        }//Slots();

        //UpdateMoney - Updates the user's current money amount
        public void UpdateMoney()
        {
            //Print to hundredth even if it's 0s
            switch (NumQuarter % 4)
            {
                case 0:
                    MoneyLabel.Text = "" + NumQuarter * 0.25 + ".00";
                    break;
                case 2:
                    MoneyLabel.Text = "" + NumQuarter * 0.25 + "0";
                    break;
                default:
                    MoneyLabel.Text = "" + NumQuarter * 0.25;
                    break;
            }//switch
        }//UpdateMoney();

        //StartButton_Click - Starts a single slots game and consumes a quarter
        private void StartButton_Click(object sender, EventArgs e)
        {
            //Only start if there isn't another game going on
            if (allowstart)
            {
                //Only start if the user has money
                if (NumQuarter > 0)
                {
                    //Take money and initialize for game
                    NumQuarter--;
                    counter = 1;
                    Delay = new System.Timers.Timer(250);
                    Delay.Elapsed += OnTimedEvent;
                    Delay.AutoReset = true;
                    Delay.Enabled = true;
                    win = CalculateWin();
                    UpdateMoney();
                    allowstart = false;
                }//if the user has money
                else
                {
                    //Print a message and quit program if the user has no more money
                    System.Windows.Forms.MessageBox.Show("Looks like you ran out of money! " +
                        "Come back later when you have more!");
                    Application.Exit();
                }//else the user don't have money
            }//if allow start
        }//StartButton_Click();

        //OnTimedEvent - What happens when the timer goes off
        private void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            //Change images based on amount of times the timer went off
            switch (counter)
            {
                case 1:
                    Slot1.Image = SlotMachine.Properties.Resources.SlotsGif;
                    break;
                case 2:
                    Slot2.Image = SlotMachine.Properties.Resources.SlotsGif;
                    Slot1.Image = SlotMachine.Properties.Resources.SlotsGifFast;
                    break;
                case 3:
                    Slot3.Image = SlotMachine.Properties.Resources.SlotsGif;
                    Slot2.Image = SlotMachine.Properties.Resources.SlotsGifFast;
                    break;
                case 4:
                    Slot3.Image = SlotMachine.Properties.Resources.SlotsGifFast;
                    break;
                case 5:
                    //Stop timer and allow push button
                    Delay.Stop();
                    Delay.Dispose();
                    counter = 0;
                    allowbutton = true;
                    break;
            }//switch
            counter++;
        }//OnTimedEvent();

        //CalculateWin - Generate a number to determine whether or not the user won
        private int CalculateWin()
        {
            int number;     //Number that will be generated
            Random random = new Random();       //Number generator

            //Generate a number
            number = random.Next(1, 1024);
            //Return based on number generated
            if (number <= 1)
                return 1;
            else
            {
                if (number <= (2 + 1))
                    return 2;
                else
                {
                    if (number <= (4 + 2 + 1))
                        return 3;
                    else
                    {
                        if (number <= (8 + 4 + 2 + 1))
                            return 4;
                        else
                        {
                            if (number <= (16 + 8 + 4 + 2 + 1))
                                return 5;
                            else
                            {
                                if (number <= (64 + 16 + 8 + 4 + 2 + 1))
                                    return 6;
                                else
                                {
                                    if (number <= (128 + 64 + 16 + 8 + 4 + 2 + 1))
                                        return 7;
                                    else
                                        return 0;
                                }//else
                            }//else
                        }//else
                    }//else
                }//else
            }//else
        }//CalculateWin();

        //PushButton_Click - Stops one of the slots if the user pushes it
        private void PushButton_Click(object sender, EventArgs e)
        {
            Random random = new Random();       //Random number generator
            int temp;               //Temporary placeholder for random images

            //if there is an ongoing game
            if (allowbutton)
            {
                if (win > 0)
                {
                    SlotPosition(counter, win);
                }//if the user won
                else
                {
                    //Pick a random image if the user didn't win
                    temp = random.Next(1, 7);
                    //Prevent the program from generating 3 of the same image
                    if (counter == 2)
                        antifalse = temp;
                    if (counter == 3)
                        while (temp == antifalse)
                            temp = random.Next(1, 7);
                    SlotPosition(counter, temp);
                }//else the user didn't win

                counter++;
                //After game is complete, add money if the user won, and allow the user to start another game
                if (counter == 4)
                {
                    //Add money if the user won
                    switch (win)
                    {
                        case 1:
                            NumQuarter += 100;
                            break;
                        case 2:
                            NumQuarter += 50;
                            break;
                        case 3:
                            NumQuarter += 25;
                            break;
                        case 4:
                            NumQuarter += 10;
                            break;
                        case 5:
                            NumQuarter += 5;
                            break;
                        case 6:
                            NumQuarter += 3;
                            break;
                        case 7:
                            NumQuarter += 2;
                            break;
                    }
                    allowbutton = false;
                    allowstart = true;
                    antifalse = -1;
                    counter = 1;
                    UpdateMoney();
                }//if game ended
            }//if push button is allowed
        }//PushButton_Click();

        //SlotPosition - Sets a certain slot
        private void SlotPosition(int slotnum, int winnum)
        {
            switch (slotnum)
            {
                case 1:
                    Slot1.Image = SlotType(winnum);
                    break;
                case 2:
                    Slot2.Image = SlotType(winnum);
                    break;
                case 3:
                    Slot3.Image = SlotType(winnum);
                    break;
            }//switch
        }//SlotPosition();

        //SlotType - Set image based on generated number
        private System.Drawing.Bitmap SlotType(int winnum)
        {
            //Choose image based on generated number
            switch (winnum)
            {
                case 1:
                    return SlotMachine.Properties.Resources.DragonFruit;
                case 2:
                    return SlotMachine.Properties.Resources.StarFruit;
                case 3:
                    return SlotMachine.Properties.Resources.Coconut;
                case 4:
                    return SlotMachine.Properties.Resources.Pineapple;
                case 5:
                    return SlotMachine.Properties.Resources.Papaya;
                case 6:
                    return SlotMachine.Properties.Resources.Mango;
                case 7:
                    return SlotMachine.Properties.Resources.Kiwi;
            }//Switch

            //Return wooden texture if error
            return SlotMachine.Properties.Resources.WoodTexure;
        }//SlotType();
    }//Slots class
}//SlotMachine namespace
