using Infragistics.Controls.Barcodes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KB10505_XamBarCodeIsValid_WpfApp1;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void button1_Click(object sender, RoutedEventArgs e)
    {
        CreateQRCodeVersion1("月火水木");
    }

    private void button2_Click(object sender, RoutedEventArgs e)
    {
        CreateQRCodeVersion1("月火水木金");
    }

    private void CreateQRCodeVersion1(string output)
    {
        var barcode = new XamQRCodeBarcode
        {
            Data = output,
            SizeVersion = SizeVersion.Version1,
            ErrorCorrectionLevel = QRCodeErrorCorrectionLevel.High,
        };
        var size = new Size(300, 300);
        barcode.Measure(size);
        barcode.Arrange(new Rect(size));

        if (!barcode.IsValid)
        {
            MessageBox.Show("有効なデータではありません");
            return;
        }

        // === 以下、コントロールの描画内容を画像にするというWPF一般の内容です。 ===
        var renderTargetBitmap = new RenderTargetBitmap((int)barcode.ActualWidth, (int)barcode.ActualHeight, 96, 96, PixelFormats.Pbgra32);
        renderTargetBitmap.Render(barcode);

        var encoder = new PngBitmapEncoder();
        encoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));

        using (var stream = new FileStream($@"{output}.png", FileMode.Create))
        {
            encoder.Save(stream);

            stream.Flush();
            stream.Close();
        }

        // revert old position
        barcode.Measure(new Size());

        MessageBox.Show("Done!");
    }

}
