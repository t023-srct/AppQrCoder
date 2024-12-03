using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace AppQrCoder
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // ตรวจสอบว่ามีข้อความใน RichTextBox หรือไม่
                if (string.IsNullOrWhiteSpace(richTextBox1.Text))
                {
                    MessageBox.Show("กรุณากรอกข้อความเพื่อสร้าง QR Code");
                    return;
                }

                // สร้าง QR Code
                QRCoder.QRCodeGenerator qr = new QRCoder.QRCodeGenerator();
                var MyData = qr.CreateQrCode(richTextBox1.Text, QRCoder.QRCodeGenerator.ECCLevel.H);
                var code = new QRCoder.QRCode(MyData);

                // สร้างภาพ QR Code
                var qrImage = code.GetGraphic(10); // ปรับขนาดของโมดูล

                // ตั้งค่าภาพใน PictureBox
                pictureBox1.Image = qrImage;

                // ปรับขนาด PictureBox ให้สัมพันธ์กับขนาดของภาพ
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox1.Width = 200; // ตั้งค่ากว้าง
                pictureBox1.Height = 200; // ตั้งค่าสูง
            }
            catch (Exception ex)
            {
                // จัดการข้อผิดพลาด
                MessageBox.Show("เกิดข้อผิดพลาด: " + ex.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            date.Text = now.ToString("dd/MM/yyyy");
            time.Text = now.ToString("HH:mm:ss");
        }


        private void buttonSave_Click_1(object sender, EventArgs e)
        {
            try
            {
                // ตรวจสอบว่ามี QR Code ใน PictureBox หรือไม่
                if (pictureBox1.Image == null)
                {
                    MessageBox.Show("กรุณาสร้าง QR Code ก่อนการบันทึก");
                    return;
                }

                // กำหนดเส้นทางการบันทึก
                string folderPath = @"D:\Temp\QR"; // หรือสามารถเปลี่ยนเป็นพาธที่ต้องการ
                string fileName = $"q_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.png";
                string filePath = Path.Combine(folderPath, fileName);

                // ตรวจสอบว่าโฟลเดอร์สามารถเข้าถึงได้หรือไม่
                if (!Directory.Exists(folderPath))
                {
                    MessageBox.Show("โฟลเดอร์ที่ระบุไม่มีอยู่หรือไม่สามารถเข้าถึงได้");
                    return;
                }

                // บันทึกภาพ QR Code เป็นไฟล์ PNG
                pictureBox1.Image.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);

                // แสดงข้อความเมื่อบันทึกสำเร็จ
                MessageBox.Show($"QR Code ได้ถูกบันทึกไว้ที่ {filePath}");
            }
            catch (Exception ex)
            {
                // แสดงข้อความข้อผิดพลาด
                MessageBox.Show("เกิดข้อผิดพลาด: " + ex.Message);
            }
        }
    }
}
