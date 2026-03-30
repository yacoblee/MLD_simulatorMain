using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static test.Form1;

namespace test
{
    public partial class Form2 : Form
    {

        Form1 _mainForm;
        public event EventHandler DrsRequestTriggered;
        public event Action<int, int> ConfigRequested;
        private Dictionary<int, Param> cel;
        private Form3 _form3;


        public Form2()
        {
            InitializeComponent();
            usable1.BackColor = Color.Green;
            usable1.BackColor = Color.White;
        }

        public void registGridData()
        {
            cel = new Dictionary<int, Param>();
            cel.Add(1, new Param("PV1", 0));
            cel.Add(2, new Param("PV2", 0));
            cel.Add(3, new Param("PV3", 0));
            cel.Add(4, new Param("PV4", 0));

            cel.Add(6, new Param("SV1", 0));
            cel.Add(7, new Param("SV2", 0));
            cel.Add(8, new Param("SV3", 0));
            cel.Add(9, new Param("SV4", 0));

            cel.Add(11, new Param("MV1", 0));
            cel.Add(12, new Param("MV2", 0));
            cel.Add(13, new Param("MV3", 0));
            cel.Add(14, new Param("MV4", 0));

            cel.Add(16, new Param("CHSTS1", 0));
            cel.Add(17, new Param("CHSTS2", 0));
            cel.Add(18, new Param("CHSTS3", 0));
            cel.Add(19, new Param("CHSTS4", 0));

            cel.Add(21, new Param("TSV1", 0));
            cel.Add(22, new Param("TSV2", 0));
            cel.Add(23, new Param("TSV3", 0));
            cel.Add(24, new Param("TSV4", 0));

            cel.Add(26, new Param("EVT_STS", 0));
            cel.Add(27, new Param("EVBUS_STS", 0));

            cel.Add(31, new Param("LOCK", 0));
            cel.Add(32, new Param("COMCHK", 0));
            cel.Add(33, new Param("PARA_SAVE", 0));
            cel.Add(34, new Param("PARA_COPY", 0));

            cel.Add(36, new Param("CHMD1", 0));
            cel.Add(37, new Param("CHMD2", 0));
            cel.Add(38, new Param("CHMD3", 0));
            cel.Add(39, new Param("CHMD4", 0));

            cel.Add(41, new Param("AT1", 0));
            cel.Add(42, new Param("AT2", 0));
            cel.Add(43, new Param("AT3", 0));
            cel.Add(44, new Param("AT4", 0));

            cel.Add(46, new Param("R/S", 0));
            cel.Add(47, new Param("R/S_SAVE", 0));
            cel.Add(48, new Param("PARA_INIT", 0));

            cel.Add(61, new Param("EVSTS1", 0));
            cel.Add(62, new Param("EVSTS2", 0));
            cel.Add(63, new Param("EVSTS3", 0));
            cel.Add(64, new Param("EVSTS4", 0));

            cel.Add(66, new Param("EV1STS", 0));
            cel.Add(67, new Param("EV2STS", 0));
            cel.Add(68, new Param("EV3STS", 0));
            cel.Add(69, new Param("EV4STS", 0));
            cel.Add(70, new Param("EV5STS", 0));
            cel.Add(71, new Param("EV6STS", 0));
            cel.Add(72, new Param("EV7STS", 0));
            cel.Add(73, new Param("EV8STS", 0));

            cel.Add(76, new Param("EVBUS1", 0));
            cel.Add(77, new Param("EVBUS2", 0));
            cel.Add(80, new Param("EVBUS3", 0));
            cel.Add(78, new Param("EVBUS4", 0));
            cel.Add(79, new Param("EVBUS5", 0));
            cel.Add(81, new Param("EVBUS6", 0));
            cel.Add(82, new Param("EVBUS7", 0));
            cel.Add(83, new Param("EVBUS8", 0));

            cel.Add(86, new Param("PAS", 0));
            cel.Add(87, new Param("BPS", 0));
            cel.Add(88, new Param("PRI", 0));
            cel.Add(89, new Param("STP", 0));
            cel.Add(90, new Param("DLN", 0));
            cel.Add(91, new Param("RPT", 0));

            cel.Add(94, new Param("DBMG_ch", 0));
            cel.Add(95, new Param("ADDR", 0));
            cel.Add(96, new Param("MAX_CH", 0));
            cel.Add(97, new Param("R_SYS", 0));
            cel.Add(98, new Param("R_OPT", 0));
            cel.Add(99, new Param("ROMVER", 0));



            cel.Add(101, new Param("PV1", 0));
            cel.Add(201, new Param("PV2", 0));
            cel.Add(301, new Param("PV3", 0));
            cel.Add(401, new Param("PV4", 0));

            cel.Add(102, new Param("SV1", 0));
            cel.Add(202, new Param("SV2", 0));
            cel.Add(302, new Param("SV3", 0));
            cel.Add(402, new Param("SV4", 0));

            cel.Add(103, new Param("MV1", 0));
            cel.Add(203, new Param("MV2", 0));
            cel.Add(303, new Param("MV3", 0));
            cel.Add(403, new Param("MV4", 0));

            cel.Add(104, new Param("CHSTS1", 0));
            cel.Add(204, new Param("CHSTS2", 0));
            cel.Add(304, new Param("CHSTS3", 0));
            cel.Add(404, new Param("CHSTS4", 0));

            cel.Add(105, new Param("EVSTS1", 0));
            cel.Add(205, new Param("EVSTS2", 0));
            cel.Add(305, new Param("EVSTS3", 0));
            cel.Add(405, new Param("EVSTS4", 0));

            cel.Add(106, new Param("OUTSTS1", 0));
            cel.Add(206, new Param("OUTSTS2", 0));
            cel.Add(306, new Param("OUTSTS3", 0));
            cel.Add(406, new Param("OUTSTS4", 0));

            cel.Add(107, new Param("RJC1", 0));
            cel.Add(207, new Param("RJC2", 0));
            cel.Add(307, new Param("RJC3", 0));
            cel.Add(407, new Param("RJC4", 0));

            cel.Add(108, new Param("TC1", 0));
            cel.Add(208, new Param("TC2", 0));
            cel.Add(308, new Param("TC3", 0));
            cel.Add(408, new Param("TC4", 0));

            cel.Add(109, new Param("INP1", 0));
            cel.Add(209, new Param("INP2", 0));
            cel.Add(309, new Param("INP3", 0));
            cel.Add(409, new Param("INP4", 0));

            cel.Add(110, new Param("CHMD1", 0));
            cel.Add(210, new Param("CHMD2", 0));
            cel.Add(310, new Param("CHMD3", 0));
            cel.Add(410, new Param("CHMD4", 0));

            // 11 ~ 12번
            cel.Add(111, new Param("AT1", 0));
            cel.Add(211, new Param("AT2", 0));
            cel.Add(311, new Param("AT3", 0));
            cel.Add(411, new Param("AT4", 0));

            cel.Add(112, new Param("OUT1", 0));
            cel.Add(212, new Param("OUT2", 0));
            cel.Add(312, new Param("OUT3", 0));
            cel.Add(412, new Param("OUT4", 0));

            // 13 ~ 20번 (EV Delay)
            cel.Add(113, new Param("EV1DLY1", 0));
            cel.Add(213, new Param("EV1DLY2", 0));
            cel.Add(313, new Param("EV1DLY3", 0));
            cel.Add(413, new Param("EV1DLY4", 0));

            cel.Add(114, new Param("EV2DLY1", 0));
            cel.Add(214, new Param("EV2DLY2", 0));
            cel.Add(314, new Param("EV2DLY3", 0));
            cel.Add(414, new Param("EV2DLY4", 0));

            cel.Add(115, new Param("EV3DLY1", 0));
            cel.Add(215, new Param("EV3DLY2", 0));
            cel.Add(315, new Param("EV3DLY3", 0));
            cel.Add(415, new Param("EV3DLY4", 0));

            cel.Add(116, new Param("EV4DLY1", 0));
            cel.Add(216, new Param("EV4DLY2", 0));
            cel.Add(316, new Param("EV4DLY3", 0));
            cel.Add(416, new Param("EV4DLY4", 0));

            cel.Add(117, new Param("EV5DLY1", 0));
            cel.Add(217, new Param("EV5DLY2", 0));
            cel.Add(317, new Param("EV5DLY3", 0));
            cel.Add(417, new Param("EV5DLY4", 0));

            cel.Add(118, new Param("EV6DLY1", 0));
            cel.Add(218, new Param("EV6DLY2", 0));
            cel.Add(318, new Param("EV6DLY3", 0));
            cel.Add(418, new Param("EV6DLY4", 0));

            cel.Add(119, new Param("EV7DLY1", 0));
            cel.Add(219, new Param("EV7DLY2", 0));
            cel.Add(319, new Param("EV7DLY3", 0));
            cel.Add(419, new Param("EV7DLY4", 0));

            cel.Add(120, new Param("EV8DLY1", 0));
            cel.Add(220, new Param("EV8DLY2", 0));
            cel.Add(320, new Param("EV8DLY3", 0));
            cel.Add(420, new Param("EV8DLY4", 0));

            // 21 ~ 44번 (EV Settings: TY, VL, HY 반복)
            cel.Add(121, new Param("EV1TY1", 0));
            cel.Add(221, new Param("EV1TY2", 0));
            cel.Add(321, new Param("EV1TY3", 0));
            cel.Add(421, new Param("EV1TY4", 0));

            cel.Add(122, new Param("EV1VL1", 0));
            cel.Add(222, new Param("EV1VL2", 0));
            cel.Add(322, new Param("EV1VL3", 0));
            cel.Add(422, new Param("EV1VL4", 0));

            cel.Add(123, new Param("EV1HY1", 0));
            cel.Add(223, new Param("EV1HY2", 0));
            cel.Add(323, new Param("EV1HY3", 0));
            cel.Add(423, new Param("EV1HY4", 0));

            cel.Add(124, new Param("EV2TY1", 0));
            cel.Add(224, new Param("EV2TY2", 0));
            cel.Add(324, new Param("EV2TY3", 0));
            cel.Add(424, new Param("EV2TY4", 0));

            cel.Add(125, new Param("EV2VL1", 0));
            cel.Add(225, new Param("EV2VL2", 0));
            cel.Add(325, new Param("EV2VL3", 0));
            cel.Add(425, new Param("EV2VL4", 0));

            cel.Add(126, new Param("EV2HY1", 0));
            cel.Add(226, new Param("EV2HY2", 0));
            cel.Add(326, new Param("EV2HY3", 0));
            cel.Add(426, new Param("EV2HY4", 0));

            cel.Add(127, new Param("EV3TY1", 0));
            cel.Add(227, new Param("EV3TY2", 0));
            cel.Add(327, new Param("EV3TY3", 0));
            cel.Add(427, new Param("EV3TY4", 0));

            cel.Add(128, new Param("EV3VL1", 0));
            cel.Add(228, new Param("EV3VL2", 0));
            cel.Add(328, new Param("EV3VL3", 0));
            cel.Add(428, new Param("EV3VL4", 0));

            cel.Add(129, new Param("EV3HY1", 0));
            cel.Add(229, new Param("EV3HY2", 0));
            cel.Add(329, new Param("EV3HY3", 0));
            cel.Add(429, new Param("EV3HY4", 0));

            cel.Add(130, new Param("EV4TY1", 0));
            cel.Add(230, new Param("EV4TY2", 0));
            cel.Add(330, new Param("EV4TY3", 0));
            cel.Add(430, new Param("EV4TY4", 0));

            cel.Add(131, new Param("EV4VL1", 0));
            cel.Add(231, new Param("EV4VL2", 0));
            cel.Add(331, new Param("EV4VL3", 0));
            cel.Add(431, new Param("EV4VL4", 0));

            cel.Add(132, new Param("EV4HY1", 0));
            cel.Add(232, new Param("EV4HY2", 0));
            cel.Add(332, new Param("EV4HY3", 0));
            cel.Add(432, new Param("EV4HY4", 0));

            cel.Add(133, new Param("EV5TY1", 0));
            cel.Add(233, new Param("EV5TY2", 0));
            cel.Add(333, new Param("EV5TY3", 0));
            cel.Add(433, new Param("EV5TY4", 0));

            cel.Add(134, new Param("EV5VL1", 0));
            cel.Add(234, new Param("EV5VL2", 0));
            cel.Add(334, new Param("EV5VL3", 0));
            cel.Add(434, new Param("EV5VL4", 0));

            cel.Add(135, new Param("EV5HY1", 0));
            cel.Add(235, new Param("EV5HY2", 0));
            cel.Add(335, new Param("EV5HY3", 0));
            cel.Add(435, new Param("EV5HY4", 0));

            cel.Add(136, new Param("EV6TY1", 0));
            cel.Add(236, new Param("EV6TY2", 0));
            cel.Add(336, new Param("EV6TY3", 0));
            cel.Add(436, new Param("EV6TY4", 0));

            cel.Add(137, new Param("EV6VL1", 0));
            cel.Add(237, new Param("EV6VL2", 0));
            cel.Add(337, new Param("EV6VL3", 0));
            cel.Add(437, new Param("EV6VL4", 0));

            cel.Add(138, new Param("EV6HY1", 0));
            cel.Add(238, new Param("EV6HY2", 0));
            cel.Add(338, new Param("EV6HY3", 0));
            cel.Add(438, new Param("EV6HY4", 0));

            cel.Add(139, new Param("EV7TY1", 0));
            cel.Add(239, new Param("EV7TY2", 0));
            cel.Add(339, new Param("EV7TY3", 0));
            cel.Add(439, new Param("EV7TY4", 0));

            cel.Add(140, new Param("EV7VL1", 0));
            cel.Add(240, new Param("EV7VL2", 0));
            cel.Add(340, new Param("EV7VL3", 0));
            cel.Add(440, new Param("EV7VL4", 0));

            cel.Add(141, new Param("EV7HY1", 0));
            cel.Add(241, new Param("EV7HY2", 0));
            cel.Add(341, new Param("EV7HY3", 0));
            cel.Add(441, new Param("EV7HY4", 0));

            cel.Add(142, new Param("EV8TY1", 0));
            cel.Add(242, new Param("EV8TY2", 0));
            cel.Add(342, new Param("EV8TY3", 0));
            cel.Add(442, new Param("EV8TY4", 0));

            cel.Add(143, new Param("EV8VL1", 0));
            cel.Add(243, new Param("EV8VL2", 0));
            cel.Add(343, new Param("EV8VL3", 0));
            cel.Add(443, new Param("EV8VL4", 0));

            cel.Add(144, new Param("EV8HY1", 0));
            cel.Add(244, new Param("EV8HY2", 0));
            cel.Add(344, new Param("EV8HY3", 0));
            cel.Add(444, new Param("EV8HY4", 0));

            // 45 ~ 47번 (LBA, LBD, EVSTOP)
            cel.Add(145, new Param("LBA1", 0));
            cel.Add(245, new Param("LBA2", 0));
            cel.Add(345, new Param("LBA3", 0));
            cel.Add(445, new Param("LBA4", 0));

            cel.Add(146, new Param("LBD1", 0));
            cel.Add(246, new Param("LBD2", 0));
            cel.Add(346, new Param("LBD3", 0));
            cel.Add(446, new Param("LBD4", 0));

            cel.Add(147, new Param("EVSTOP1", 0));
            cel.Add(247, new Param("EVSTOP2", 0));
            cel.Add(347, new Param("EVSTOP3", 0));
            cel.Add(447, new Param("EVSTOP4", 0));

            // 48 ~ 50번 없음

            // 51 ~ 66번
            cel.Add(151, new Param("PB1", 0));
            cel.Add(251, new Param("PB2", 0));
            cel.Add(351, new Param("PB3", 0));
            cel.Add(451, new Param("PB4", 0));

            cel.Add(152, new Param("TI1", 0));
            cel.Add(252, new Param("TI2", 0));
            cel.Add(352, new Param("TI3", 0));
            cel.Add(452, new Param("TI4", 0));

            cel.Add(153, new Param("TD1", 0));
            cel.Add(253, new Param("TD2", 0));
            cel.Add(353, new Param("TD3", 0));
            cel.Add(453, new Param("TD4", 0));

            cel.Add(154, new Param("AP1", 0));
            cel.Add(254, new Param("AP2", 0));
            cel.Add(354, new Param("AP3", 0));
            cel.Add(454, new Param("AP4", 0));

            cel.Add(155, new Param("MR1", 0));
            cel.Add(255, new Param("MR2", 0));
            cel.Add(355, new Param("MR3", 0));
            cel.Add(455, new Param("MR4", 0));

            cel.Add(156, new Param("CT1", 0));
            cel.Add(256, new Param("CT2", 0));
            cel.Add(356, new Param("CT3", 0));
            cel.Add(456, new Param("CT4", 0));

            cel.Add(157, new Param("PO1", 0));
            cel.Add(257, new Param("PO2", 0));
            cel.Add(357, new Param("PO3", 0));
            cel.Add(457, new Param("PO4", 0));

            cel.Add(158, new Param("HYS1", 0));
            cel.Add(258, new Param("HYS2", 0));
            cel.Add(358, new Param("HYS3", 0));
            cel.Add(458, new Param("HYS4", 0));

            cel.Add(159, new Param("RO1", 0));
            cel.Add(259, new Param("RO2", 0));
            cel.Add(359, new Param("RO3", 0));
            cel.Add(459, new Param("RO4", 0));

            cel.Add(160, new Param("RUP1", 0));
            cel.Add(260, new Param("RUP2", 0));
            cel.Add(360, new Param("RUP3", 0));
            cel.Add(460, new Param("RUP4", 0));

            cel.Add(161, new Param("RDN1", 0));
            cel.Add(261, new Param("RDN2", 0));
            cel.Add(361, new Param("RDN3", 0));
            cel.Add(461, new Param("RDN4", 0));

            cel.Add(162, new Param("RMIN1", 0));
            cel.Add(262, new Param("RMIN2", 0));
            cel.Add(362, new Param("RMIN3", 0));
            cel.Add(462, new Param("RMIN4", 0));

            cel.Add(163, new Param("RHRS1", 0));
            cel.Add(263, new Param("RHRS2", 0));
            cel.Add(363, new Param("RHRS3", 0));
            cel.Add(463, new Param("RHRS4", 0));

            cel.Add(164, new Param("DR1", 0));
            cel.Add(264, new Param("DR2", 0));
            cel.Add(364, new Param("DR3", 0));
            cel.Add(464, new Param("DR4", 0));

            cel.Add(165, new Param("OHL1", 0));
            cel.Add(265, new Param("OHL2", 0));
            cel.Add(365, new Param("OHL3", 0));
            cel.Add(465, new Param("OHL4", 0));

            cel.Add(166, new Param("OLL1", 0));
            cel.Add(266, new Param("OLL2", 0));
            cel.Add(366, new Param("OLL3", 0));
            cel.Add(466, new Param("OLL4", 0));

            // 67 ~ 70번 없음

            // 71 ~ 79번
            cel.Add(171, new Param("BS1", 0));
            cel.Add(271, new Param("BS2", 0));
            cel.Add(371, new Param("BS3", 0));
            cel.Add(471, new Param("BS4", 0));

            cel.Add(172, new Param("FL1", 0));
            cel.Add(272, new Param("FL2", 0));
            cel.Add(372, new Param("FL3", 0));
            cel.Add(472, new Param("FL4", 0));

            cel.Add(173, new Param("SVH1", 0));
            cel.Add(273, new Param("SVH2", 0));
            cel.Add(373, new Param("SVH3", 0));
            cel.Add(473, new Param("SVH4", 0));

            cel.Add(174, new Param("SVL1", 0));
            cel.Add(274, new Param("SVL2", 0));
            cel.Add(374, new Param("SVL3", 0));
            cel.Add(474, new Param("SVL4", 0));

            cel.Add(175, new Param("FRH1", 0));
            cel.Add(275, new Param("FRH2", 0));
            cel.Add(375, new Param("FRH3", 0));
            cel.Add(475, new Param("FRH4", 0));

            cel.Add(176, new Param("FRL1", 0));
            cel.Add(276, new Param("FRL2", 0));
            cel.Add(376, new Param("FRL3", 0));
            cel.Add(476, new Param("FRL4", 0));

            cel.Add(177, new Param("SLH1", 0));
            cel.Add(277, new Param("SLH2", 0));
            cel.Add(377, new Param("SLH3", 0));
            cel.Add(477, new Param("SLH4", 0));

            cel.Add(178, new Param("SLL1", 0));
            cel.Add(278, new Param("SLL2", 0));
            cel.Add(378, new Param("SLL3", 0));
            cel.Add(478, new Param("SLL4", 0));

            cel.Add(179, new Param("DOT1", 0));
            cel.Add(279, new Param("DOT2", 0));
            cel.Add(379, new Param("DOT3", 0));
            cel.Add(479, new Param("DOT4", 0));




        }


        private void Form2_Load(object sender, EventArgs e)
        {
            registGridData();
            dataGridView2.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView2.ReadOnly = true;
            dataGridView2.AllowUserToAddRows = false;
            dataGridView2.Rows.Clear();
            dataGridView2.Columns[0].Width = 60;
            for (int i = 1; i < 11; i++)
            {

                if (i % 2 == 0)
                {
                    dataGridView2.Columns[i].Width = 70;
                    // 값 색상
                    dataGridView2.Columns[i].DefaultCellStyle.BackColor = Color.LightGray;
                }
                else
                {
                    // 이름 색상
                    dataGridView2.Columns[i].DefaultCellStyle.BackColor = Color.Gray;
                }
            }


            for (int i = 0; i < 100; i++)
            {
                dataGridView2.Rows.Add();
                dataGridView2.Rows[i].Cells[0].Value = i.ToString("00");
                

                if (cel.Keys.Contains(i))
                {
                    dataGridView2.Rows[i].Cells[1].Value = cel[i].Name;
                    dataGridView2.Rows[i].Cells[2].Value = cel[i].Value;

                }

                if (cel.Keys.Contains(i + 100))
                {
                    dataGridView2.Rows[i].Cells[3].Value = cel[i + 100].Name;
                    dataGridView2.Rows[i].Cells[4].Value = cel[i + 100].Value;
                }

                if (cel.Keys.Contains(i + 200))
                {
                    dataGridView2.Rows[i].Cells[5].Value = cel[i + 200].Name;
                    dataGridView2.Rows[i].Cells[6].Value = cel[i + 200].Value;
                }

                if (cel.Keys.Contains(i + 300))
                {
                    dataGridView2.Rows[i].Cells[7].Value = cel[i + 300].Name;
                    dataGridView2.Rows[i].Cells[8].Value = cel[i + 300].Value;
                }


                if (cel.Keys.Contains(i + 400))
                {
                    dataGridView2.Rows[i].Cells[9].Value = cel[i + 400].Name;
                    dataGridView2.Rows[i].Cells[10].Value = cel[i + 400].Value;
                }
            }

        
        }


        public void DataReceived(object sender, DrsDataEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => DataReceived(sender, e)));
                return;
            }

            foreach (var item in e.DataList)
            {
                if (cel.ContainsKey(item.idx))
                {
                    int rowIndex = item.idx % 100;
                    int colIndex = (item.idx / 100) * 2 + 2;

                    if (configData.dic.ContainsKey(item.idx))
                    {
                        item.data = TransPvValue(item.data, configData.dic[item.idx].Value);
                    }
                    dataGridView2.Rows[rowIndex].Cells[colIndex].Value = item.data;

                    if (configData.dic.ContainsKey(item.idx))
                    {
                        dataGridView2.Rows[rowIndex].Cells[colIndex].Style.BackColor = Color.Green;
                    }
                    else
                    {
                        dataGridView2.Rows[rowIndex].Cells[colIndex].Style.BackColor = Color.LightGray;
                    }
                }
            }
        }
        private double TransPvValue(double rawData, double configValue)
        {
            if (configValue != 0 && configValue % 2 == 0)
            {
                return rawData / 10.0;  
            }
            return rawData;
        }

        private void drsBtn_Click(object sender, EventArgs e)
        {
            DrsRequestTriggered?.Invoke(this, EventArgs.Empty);


     
        }

        private void label9_Click(object sender, EventArgs e)
        {
            
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            //saveBtn

       
        }

        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;
            int colIndex = e.ColumnIndex;
            int idx = (rowIndex) + ((colIndex - 2)/2  * 100);
            textBox1.Text += $"{e.RowIndex} col : {e.ColumnIndex} cell \r\n{(e.ColumnIndex - 2) / 2 * 100 + e.RowIndex}\n";
            
            
            if ((dataGridView2.Rows[rowIndex].Cells[colIndex].Style.BackColor == Color.Green)
                && configData.dic.ContainsKey(idx))
            {
                Form3 popup = new Form3(idx);
                popup.StartPosition = FormStartPosition.CenterParent;
                popup.ShowDialog();


                if (popup.ShowDialog() == DialogResult.OK)
                {
                    int updateAddr = popup.TargetIdx;
                    int updateVal = popup.TargetValue;

                    ConfigRequested?.Invoke(updateAddr, updateVal);
                }
          


            }

        }
    }
}

//ConfigRequested
