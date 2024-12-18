using SFML.Audio;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Diagnostics;
using System.Net;
using LibVLCSharp;
using LibVLCSharp.Forms;
using LibVLCSharp.Shared;
using LibVLCSharp.WinForms;
using Xamarin.Forms.PlatformConfiguration;

namespace DnD_music_program
{
    internal class OverlayUI
    {
        private PictureBox mumbo = new PictureBox()
        {
            Size = new Size(612, 648),
            Image = Image.FromFile(Path.Combine(Directory.GetCurrentDirectory(), "textures\\mumbo.png")),
            Location = new Point(0, 0),
            Visible = false,
        };

        private VideoView video = new VideoView()
        {
            BackColor = Color.White,
            Size = new Size(1000,1000),
            Location = new Point(0, 0),
            Dock = DockStyle.Fill,
            Visible = false,
        };

        private LibVLC _LibVLC;
        private MediaPlayer _mp;
        private Media media;
        

        public OverlayUI()
        {
            Program.overlayForm.Location = new Point(0, 0);
            Program.overlayForm.Width = 1920;
            Program.overlayForm.Height = 1020;
            Program.overlayForm.TopMost = true;
            Program.overlayForm.TransparencyKey = Color.Lime;
            Program.overlayForm.BackColor = Color.Lime;
            Program.overlayForm.ShowInTaskbar = false;
            Program.overlayForm.WindowState = FormWindowState.Maximized;
            Program.overlayForm.FormBorderStyle = FormBorderStyle.None;

            mumbo.Location = new Point(Program.overlayForm.Width / 2 - 306, Program.overlayForm.Height / 2 - 324);

            Program.overlayForm.Controls.Add(video);
            Program.overlayForm.Controls.Add(mumbo);
        }

        public void Mumbo(string imageName)
        {
            // Check if Invoke is required (i.e., if we're not on the UI thread)
            if (Program.overlayForm.InvokeRequired)
            {
                // Use Invoke to marshal the UI update to the UI thread
                Program.overlayForm.Invoke(new Action(() =>
                {
                    mumbo.Image = Image.FromFile(Path.Combine(Directory.GetCurrentDirectory(), "textures\\", imageName));
                    if (mumbo.Visible)
                    {
                        mumbo.Visible = false;
                    }
                    else
                    {
                        mumbo.Visible = true;
                    }
                }));
            }
            else
            {
                // If we're already on the UI thread, do the UI updates directly
                if (mumbo.Visible)
                {
                    mumbo.Visible = false;
                }
                else
                {
                    mumbo.Visible = true;
                }
            }
        }

        public void Video()
        {
            // Check if Invoke is required (i.e., if we're not on the UI thread)
            if (Program.overlayForm.InvokeRequired)
            {
                // Use Invoke to marshal the UI update to the UI thread
                Program.overlayForm.Invoke(new Action(() =>
                {
                    //prevents from trying to play two videos at once
                    if(video.MediaPlayer != null && video.MediaPlayer.IsPlaying)
                    {
                        Debug.WriteLine("still playing");
                        return;
                    }
                    _LibVLC = new LibVLC(enableDebugLogs: true);
                    _mp = new MediaPlayer(_LibVLC);
                    

                    video.MediaPlayer = _mp;

                    string path = Path.Combine(Directory.GetCurrentDirectory(), "videos\\mouse.mov");

                    _mp.Media = new Media(_LibVLC, path, FromType.FromPath);

                    video.Visible = true;

                    //jump to another thread to run video so that ui thread is not interupted
                    Task.Run(() =>
                    {
                        video.MediaPlayer.Play();

                        //for some reason Length is not updated before continuing execution so delay for 100ms to let it set
                        //find a better solution later.
                        Task.Delay(100).Wait();

                        //delay the thread for as long as the video is
                        Task.Delay((int)video.MediaPlayer.Length).Wait();

                        video.MediaPlayer.Pause();
                        video.MediaPlayer.Stop();
                        video.MediaPlayer.Dispose();

                        //need to jump back to UI thread in order to disable form related stuff
                        Program.overlayForm.Invoke(new Action(() =>
                        {
                            video.Visible = false;
                            Program.overlayForm.Update();
                        }));
                    });  
                }));
            }
            else
            {
                //if this is reached then copy and paste code above to here
                throw new Exception("called from UI thread somehow");
            }
        }
    }
}
