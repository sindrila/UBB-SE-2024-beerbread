﻿namespace app
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            this.MainPage = new AppShell();
        }
    }
}
