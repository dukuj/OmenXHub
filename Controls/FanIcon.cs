// FanIcon.cs - 自定义风扇矢量图标（空心线性版）
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Wpf.Ui.Controls;

namespace OmenSuperHub.Controls {
  public class FanIcon : IconElement {
    // 单片空心叶片轮廓（朝上），通过 90° 旋转复制 4 片实现完美对称
    private const string BladePath =
      "M12,10 V4.5 C12,3.5 12.9,2.8 13.9,3.1 C16.5,3.8 18.5,5.9 18.5,8.3 C18.5,10.1 17.3,11.4 15.5,12";

    private static Geometry CreateFanGeometry() {
      var group = new GeometryGroup();

      // 4 片空心叶片，每片旋转 90°
      for (int i = 0; i < 4; i++) {
        var blade = Geometry.Parse(BladePath).Clone();
        blade.Transform = new RotateTransform(i * 90, 12, 12);
        group.Children.Add(blade);
      }

      // 中心空心轴心圆
      group.Children.Add(new EllipseGeometry(new Point(12, 12), 2.0, 2.0));

      return group;
    }

    protected override UIElement InitializeChildren() {
      var path = new System.Windows.Shapes.Path {
        Data = CreateFanGeometry(),
        Stretch = Stretch.Uniform,
        Fill = Brushes.Transparent, // 内部透明空心
        StrokeThickness = 1.75,     // 对齐上下图标的线条粗度
        StrokeStartLineCap = PenLineCap.Round,
        StrokeEndLineCap = PenLineCap.Round,
        StrokeLineJoin = PenLineJoin.Round,
        Margin = new Thickness(3),  // 留出与 SymbolIcon 一致的呼吸边距
        HorizontalAlignment = HorizontalAlignment.Center,
        VerticalAlignment = VerticalAlignment.Center,
        UseLayoutRounding = true,
      };
      // 绑定 Foreground 到 Stroke（线条颜色跟随主题）
      path.SetBinding(System.Windows.Shapes.Path.StrokeProperty,
        new Binding("Foreground") { Source = this });
      path.SetBinding(System.Windows.FrameworkElement.WidthProperty,
        new Binding("IconSize") { Source = this });
      path.SetBinding(System.Windows.FrameworkElement.HeightProperty,
        new Binding("IconSize") { Source = this });
      return path;
    }

    public double IconSize {
      get { return (double)GetValue(IconSizeProperty); }
      set { SetValue(IconSizeProperty, value); }
    }

    public static readonly DependencyProperty IconSizeProperty =
      DependencyProperty.Register("IconSize", typeof(double), typeof(FanIcon),
        new PropertyMetadata(28.0));
  }
}