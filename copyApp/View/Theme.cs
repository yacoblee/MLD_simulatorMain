using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace copyApp.View
{
    // camProject(BlackWhite.xaml)와 같은 팔레트를 WinForms 컨트롤에 입히는 도우미.
    // 사용: 폼 생성자에서 InitializeComponent() 다음에 Theme.Apply(this);
    // 개별 버튼 역할 지정은 Apply 뒤에 Theme.Secondary(btn) / Theme.Danger(btn)
    public static class Theme
    {
        // 정적 필드 이름이 Panel/Border처럼 WinForms 타입과 같으면 `is Panel`이 모호해지므로 접미사를 붙임
        public static readonly Color BgColor      = ColorTranslator.FromHtml("#14161F");
        public static readonly Color SurfaceColor = ColorTranslator.FromHtml("#1D2028");
        public static readonly Color BorderColor  = ColorTranslator.FromHtml("#2E3240");
        public static readonly Color TextColor    = ColorTranslator.FromHtml("#E4E6EB");
        public static readonly Color SubTextColor = ColorTranslator.FromHtml("#9098AA");
        public static readonly Color AccentColor  = ColorTranslator.FromHtml("#4C8BF5");
        public static readonly Color DangerColor  = ColorTranslator.FromHtml("#D9534F");

        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int value, int size);

        private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;   // Windows 10 20H1 이상, Windows 11

        public static void Apply(Form form)
        {
            form.BackColor = BgColor;
            form.ForeColor = TextColor;
            form.Font = new Font("Malgun Gothic", 9F);

            // 핸들이 만들어진 뒤에야 제목표시줄 속성을 줄 수 있음
            form.HandleCreated += delegate
            {
                int on = 1;
                DwmSetWindowAttribute(form.Handle, DWMWA_USE_IMMERSIVE_DARK_MODE, ref on, sizeof(int));
            };

            ApplyChildren(form);
        }

        // 기본은 파란 주 버튼. 보조 동작(저장, 취소 등)은 어두운 버튼으로 구분
        public static void Secondary(Button b)
        {
            StyleButton(b, SurfaceColor, TextColor);
            b.FlatAppearance.BorderSize = 1;
            b.FlatAppearance.BorderColor = BorderColor;
            b.FlatAppearance.MouseOverBackColor = BorderColor;
            b.FlatAppearance.MouseDownBackColor = BgColor;
        }

        // 연결 해제, 중지처럼 되돌리는 동작은 빨간 버튼
        public static void Danger(Button b)
        {
            StyleButton(b, DangerColor, Color.White);
        }

        private static void StyleButton(Button b, Color back, Color fore)
        {
            b.FlatStyle = FlatStyle.Flat;
            b.UseVisualStyleBackColor = false;   // 디자이너가 true로 두면 BackColor가 무시됨
            b.BackColor = back;
            b.ForeColor = fore;
            b.FlatAppearance.BorderSize = 0;
            b.FlatAppearance.MouseOverBackColor = ControlPaint.Light(back, 0.2f);
            b.FlatAppearance.MouseDownBackColor = ControlPaint.Dark(back, 0.1f);
            b.Cursor = Cursors.Hand;
        }

        private static void ApplyChildren(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                Style(c);
                if (c.HasChildren) ApplyChildren(c);
            }
        }

        private static void Style(Control c)
        {
            if (c is Button b)
            {
                StyleButton(b, AccentColor, Color.White);
            }
            else if (c is TextBox t)
            {
                t.BorderStyle = BorderStyle.FixedSingle;
                t.BackColor = SurfaceColor;
                t.ForeColor = TextColor;
                if (t.Multiline) t.Font = new Font("Consolas", 9F);   // 로그 창은 고정폭 글꼴
            }
            else if (c is ComboBox cb)
            {
                cb.FlatStyle = FlatStyle.Flat;
                cb.BackColor = SurfaceColor;
                cb.ForeColor = TextColor;
            }
            else if (c is DataGridView g)
            {
                g.EnableHeadersVisualStyles = false;   // 없으면 헤더 색 설정이 전부 무시됨
                g.BackgroundColor = BgColor;
                g.GridColor = BorderColor;
                g.BorderStyle = BorderStyle.None;
                g.ColumnHeadersDefaultCellStyle.BackColor = SurfaceColor;
                g.ColumnHeadersDefaultCellStyle.ForeColor = TextColor;
                g.ColumnHeadersDefaultCellStyle.SelectionBackColor = SurfaceColor;
                g.RowHeadersDefaultCellStyle.BackColor = SurfaceColor;
                g.RowHeadersDefaultCellStyle.ForeColor = TextColor;
                g.DefaultCellStyle.BackColor = SurfaceColor;
                g.DefaultCellStyle.ForeColor = TextColor;
                g.DefaultCellStyle.SelectionBackColor = AccentColor;
                g.DefaultCellStyle.SelectionForeColor = Color.White;
            }
            else if (c is RadioButton || c is CheckBox)
            {
                c.ForeColor = TextColor;
            }
            else if (c is Label)
            {
                c.BackColor = Color.Transparent;   // 부모(폼/패널) 색이 그대로 비치게 함
                c.ForeColor = TextColor;
            }
            else if (c is Panel || c is GroupBox)
            {
                c.BackColor = SurfaceColor;
                c.ForeColor = TextColor;
            }
        }
    }
}
