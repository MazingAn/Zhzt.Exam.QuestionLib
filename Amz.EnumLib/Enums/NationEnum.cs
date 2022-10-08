using Amz.EnumLib.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Amz.EnumLib.Enums
{
    public enum NationEnum
    {
        [EnumDetail(Code = 1, Name = "汉")]
        HAN,
        [EnumDetail(Code = 2, Name = "蒙古")]
        MENGGU,
        [EnumDetail(Code = 3, Name = "回")]
        HUI,
        [EnumDetail(Code = 4, Name = "藏")]
        ZANG,
        [EnumDetail(Code = 5, Name = "维吾尔")]
        WEIWUER,
        [EnumDetail(Code = 6, Name = "苗")]
        MIAO,
        [EnumDetail(Code = 7, Name = "彝")]
        YI,
        [EnumDetail(Code = 8, Name = "壮")]
        ZHUANG,
        [EnumDetail(Code = 9, Name = "布依")]
        BUYI,
        [EnumDetail(Code = 10, Name = "朝鲜")]
        CHAOXIAN,
        [EnumDetail(Code = 11, Name = "满")]
        MAN,
        [EnumDetail(Code = 12, Name = "侗")]
        DONG,
        [EnumDetail(Code = 13, Name = "瑶")]
        YAO,
        [EnumDetail(Code = 14, Name = "白")]
        BAI,
        [EnumDetail(Code = 15, Name = "土家")]
        TUJIA,
        [EnumDetail(Code = 16, Name = "哈尼")]
        HANI,
        [EnumDetail(Code = 17, Name = "哈萨克")]
        KAZAK,
        [EnumDetail(Code = 18, Name = "傣")]
        DAI,
        [EnumDetail(Code = 19, Name = "黎")]
        LI,
        [EnumDetail(Code = 20, Name = "傈僳")]
        LISU,
        [EnumDetail(Code = 21, Name = "佤")]
        WA,
        [EnumDetail(Code = 22, Name = "畲")]
        SHE,
        [EnumDetail(Code = 23, Name = "高山")]
        GAOSHAN,
        [EnumDetail(Code = 24, Name = "拉祜")]
        LAHU,
        [EnumDetail(Code = 25, Name = "水")]
        SUI,
        [EnumDetail(Code = 26, Name = "东乡")]
        DONGXIANG,
        [EnumDetail(Code = 27, Name = "纳西")]
        NAXI,
        [EnumDetail(Code = 28, Name = "景颇")]
        JINGPO,
        [EnumDetail(Code = 29, Name = "柯尔克孜")]
        KEERKEZI,
        [EnumDetail(Code = 30, Name = "土")]
        TU,
        [EnumDetail(Code = 31, Name = "达斡尔")]
        DAHANER,
        [EnumDetail(Code = 32, Name = "仫佬")]
        MULAO,
        [EnumDetail(Code = 33, Name = "羌")]
        QIANG,
        [EnumDetail(Code = 34, Name = "布朗")]
        BULANG,
        [EnumDetail(Code = 35, Name = "撒拉")]
        SALA,
        [EnumDetail(Code = 36, Name = "毛南")]
        MAONAN,
        [EnumDetail(Code = 37, Name = "仡佬")]
        GELAO,
        [EnumDetail(Code = 38, Name = "锡伯")]
        XIBO,
        [EnumDetail(Code = 39, Name = "阿昌")]
        ACHANG,
        [EnumDetail(Code = 40, Name = "普米")]
        PUMI,
        [EnumDetail(Code = 41, Name = "塔吉克")]
        TAJIKE,
        [EnumDetail(Code = 42, Name = "怒")]
        NU,
        [EnumDetail(Code = 43, Name = "乌孜别克")]
        WUZIBIEKE,
        [EnumDetail(Code = 44, Name = "俄罗斯")]
        ELUOSI,
        [EnumDetail(Code = 45, Name = "鄂温克")]
        EWENKI,
        [EnumDetail(Code = 46, Name = "德昂")]
        DEANG,
        [EnumDetail(Code = 47, Name = "保安")]
        BONAN,
        [EnumDetail(Code = 48, Name = "裕固")]
        YUGU,
        [EnumDetail(Code = 49, Name = "京")]
        JING,
        [EnumDetail(Code = 50, Name = "塔塔尔")]
        TATAER,
        [EnumDetail(Code = 51, Name = "独龙")]
        DULONG,
        [EnumDetail(Code = 52, Name = "鄂伦春")]
        ELUNCHUN,
        [EnumDetail(Code = 53, Name = "赫哲")]
        HEZHEN,
        [EnumDetail(Code = 54, Name = "门巴")]
        MONBA,
        [EnumDetail(Code = 55, Name = "珞巴")]
        LUOBA,
        [EnumDetail(Code = 56, Name = "基诺")]
        JINUO,
    }
}
