namespace DnD_music_program
{
    using LibVLCSharp.Forms.Shared;
    using LibVLCSharp.Shared;

    internal static class Program
    {
        public static Form1 mainForm = new Form1();

        public static Form1 overlayForm = new Form1();

        public static UI uI = new UI();

        public static OverlayUI oUI = new OverlayUI();

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Core.Initialize(@"C:\cpts\DnD music program\DnD music program\bin\Debug\net9.0-windows\libvlc\win-x64");

            LibVLCSharpFormsRenderer.Init();

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            

            AudioManager.InitTracks();

            AudioPlayer player = new AudioPlayer();

            Task.Run(() => Application.Run(overlayForm));
            Application.Run(mainForm);
            

            overlayForm.Close();
        }
    }
}