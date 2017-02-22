using System.Collections.Generic;
using WotDossier.Domain.Tank;

namespace WotDossier.Dal
{
    public class ClientVersionCompabilityHelper
    {
        public static TankDescription GetHDModelDescription(string iconId, Dictionary<int, TankDescription> tankDescriptions)
        {
            TankDescription tankDescription = null;
            if (iconId == "ussr_t_34")
            {
                //replay tank name t_34 changed to r04_t_34
                tankDescription = tankDescriptions[0];
            }
            else if (iconId == "ussr_object_704")
            {
                //replay tank name object_704 changed to r53_object_704
                tankDescription = tankDescriptions[32];
            }
            else if (iconId == "ussr_is_4")
            {
                //0.9.7 replay tank name is_4 changed to r90_is_4m
                tankDescription = tankDescriptions[24];
            }
            else if (iconId == "usa_pershing")
            {
                //0.9.7 replay tank name pershing changed to a35_pershing
                tankDescription = tankDescriptions[20023];
            }
            else if (iconId == "usa_t26_e4_superpershing")
            {
                //0.9.7 replay tank name t26_e4_superpershing changed to a80_t26_e4_superpershing
                tankDescription = tankDescriptions[20052];
            }
            else if (iconId == "usa_t37")
            {
                //0.9.7 replay tank name changed to a94_t37
                tankDescription = tankDescriptions[20065];
            }

            else if (iconId == "germany_hummel")
            {
                //0.9.8 replay tank name changed to g02_hummel
                tankDescription = tankDescriptions[10001];
            }
            else if (iconId == "germany_leopard1")
            {
                //0.9.8 replay tank name changed to g89_leopard1
                tankDescription = tankDescriptions[10057];
            }
            else if (iconId == "france_amx_50_120")
            {
                //0.9.8 replay tank name changed to f09_amx_50_120
                tankDescription = tankDescriptions[40015];
            }

            else if (iconId == "ussr_su_85")
            {
                //0.9.8 replay tank name changed to r02_su_85
                tankDescription = tankDescriptions[1];
            }

            else if (iconId == "ussr_is_3")
            {
                //0.9.8 replay tank name changed to r19_is_3
                tankDescription = tankDescriptions[21];
            }
            else if (iconId == "usa_t32")
            {
                //0.9.8 replay tank name changed to a12_t32
                tankDescription = tankDescriptions[20017];
            }
            else if (iconId == "usa_a30_m10_wolverine")
            {
                //0.9.8 replay tank name changed to a12_t32
                tankDescription = tankDescriptions[20017];
            }

            else if (iconId == "usa_sherman_jumbo")
            {
                //0.9.8 replay tank name changed to a36_sherman_jumbo
                tankDescription = tankDescriptions[20039];
            }
            else if (iconId == "usa_m48a1")
            {
                //0.9.8 replay tank name changed to a84_m48a1
                tankDescription = tankDescriptions[20055];
            }
            else if (iconId == "france_amx_50_100")
            {
                //0.9.9 replay tank name changed to f08_amx_50_100
                tankDescription = tankDescriptions[40012];
            }
            else if (iconId == "france_fcm_50t")
            {
                //0.9.9 replay tank name changed to f65_fcm_50t
                tankDescription = tankDescriptions[40250];
            }
            else if (iconId == "france_amx_50_100_igr")
            {
                //0.9.9 replay tank name changed to f08_amx_50_100_igr
                tankDescription = tankDescriptions[40153];
            }
            else if (iconId == "germany_vk3601h")
            {
                //0.9.9 replay tank name changed to g15_vk3601h
                tankDescription = tankDescriptions[10009];
            }
            else if (iconId == "germany_panther_m10")
            {
                //0.9.9 replay tank name changed to g78_panther_m10
                tankDescription = tankDescriptions[10225];
            }
            else if (iconId == "usa_m46_patton")
            {
                //0.9.9 replay tank name changed to a63_m46_patton
                tankDescription = tankDescriptions[20035];
            }
            else if (iconId == "usa_t23e3")
            {
                //0.9.9 replay tank name changed to a86_t23e3
                tankDescription = tankDescriptions[20046];
            }
            else if (iconId == "usa_t110e4")
            {
                //0.9.9 replay tank name changed to a83_t110e4
                tankDescription = tankDescriptions[20051];
            }
            else if (iconId == "usa_t110e3")
            {
                //0.9.9 replay tank name changed to a85_t110e3
                tankDescription = tankDescriptions[20054];
            }
            else if (iconId == "usa_m6a2e1")
            {
                //0.9.9 replay tank name changed to a45_m6a2e1
                tankDescription = tankDescriptions[20205];
            }
            else if (iconId == "ussr_t62a")
            {
                //0.9.9 replay tank name changed to r87_t62a
                tankDescription = tankDescriptions[54];
            }
            else if (iconId == "france_arl_44")
            {
                //0.9.10 replay tank name changed to f06_arl_44
                tankDescription = tankDescriptions[40010];
            }
            else if (iconId == "france_amx_m4_1945")
            {
                //0.9.10 replay tank name changed to f07_amx_m4_1945
                tankDescription = tankDescriptions[40027];
            }
            else if (iconId == "germany_pz35t")
            {
                //0.9.10 replay tank name changed to g07_pz35t
                tankDescription = tankDescriptions[10003];
            }
            else if (iconId == "germany_jagdpziv")
            {
                //0.9.10 replay tank name changed to g17_jagdpziv
                tankDescription = tankDescriptions[10006];
            }
            else if (iconId == "germany_panzerjager_i")
            {
                //0.9.10 replay tank name changed to g21_panzerjager_i
                tankDescription = tankDescriptions[10014];
            }
            else if (iconId == "germany_sturmpanzer_ii")
            {
                //0.9.10 replay tank name changed to g22_sturmpanzer_ii
                tankDescription = tankDescriptions[10018];
            }
            else if (iconId == "germany_vk1602")
            {
                //0.9.10 replay tank name changed to g26_vk1602
                tankDescription = tankDescriptions[10021];
            }
            else if (iconId == "germany_jagdtiger")
            {
                //0.9.10 replay tank name changed to g44_jagdtiger
                tankDescription = tankDescriptions[10031];
            }
            else if (iconId == "germany_pro_ag_a")
            {
                //0.9.10 replay tank name changed to g91_pro_ag_a
                tankDescription = tankDescriptions[10058];
            }
            else if (iconId == "germany_pzii_j")
            {
                //0.9.10 replay tank name changed to g36_pzii_j
                tankDescription = tankDescriptions[10202];
            }
            else if (iconId == "usa_m2_lt")
            {
                //0.9.10 replay tank name changed to a02_m2_lt
                tankDescription = tankDescriptions[20007];
            }
            else if (iconId == "usa_m41")
            {
                //0.9.10 replay tank name changed to a18_m41
                tankDescription = tankDescriptions[20016];
            }
            else if (iconId == "usa_t110")
            {
                //0.9.10 replay tank name changed to a69_t110e5
                tankDescription = tankDescriptions[20042];
            }
            else if (iconId == "usa_t71")
            {
                //0.9.10 replay tank name changed to a103_t71e1
                tankDescription = tankDescriptions[20061];
            }
            else if (iconId == "usa_t71_igr")
            {
                //0.9.10 replay tank name changed to a103_t71e1_igr
                tankDescription = tankDescriptions[20151];
            }
            else if (iconId == "ussr_t_28")
            {
                //0.9.10 replay tank name changed to r06_t_28
                tankDescription = tankDescriptions[6];
            }
            else if (iconId == "ussr_su_76")
            {
                //0.9.10 replay tank name changed to r24_su_76
                tankDescription = tankDescriptions[25];
            }
            else if (iconId == "ussr_su122a")
            {
                //0.9.10 replay tank name changed to r100_su122a
                tankDescription = tankDescriptions[64];
            }
            else if (iconId == "france_amx_12t")
            {
                //0.9.12 replay tank name changed to f15_amx_12t
                tankDescription = tankDescriptions[40025];
            }
            else if (iconId == "france_amx_ac_mle1946")
            {
                //0.9.12 replay tank name changed to f35_amx_ac_mle1946
                tankDescription = tankDescriptions[40042];
            }
            else if (iconId == "germany_pzii")
            {
                //0.9.12 replay tank name changed to g06_pzii
                tankDescription = tankDescriptions[10008];
            }
            else if (iconId == "germany_bison_i")
            {
                //0.9.12 replay tank name changed to g11_bison_i
                tankDescription = tankDescriptions[10011];
            }
            else if (iconId == "germany_pzvib_tiger_ii")
            {
                //0.9.12 replay tank name changed to g16_pzvib_tiger_ii
                tankDescription = tankDescriptions[10020];
            }
            else if (iconId == "germany_grille")
            {
                //0.9.12 replay tank name changed to g23_grille
                tankDescription = tankDescriptions[10022];
            }
            else if (iconId == "germany_wespe")
            {
                //0.9.12 replay tank name changed to g19_wespe
                tankDescription = tankDescriptions[10023];
            }
            else if (iconId == "germany_pz38_na")
            {
                //0.9.12 replay tank name changed to g52_pz38_na
                tankDescription = tankDescriptions[10032];
            }
            else if (iconId == "germany_marder_iii")
            {
                //0.9.12 replay tank name changed to g39_marder_iii
                tankDescription = tankDescriptions[10044];
            }
            else if (iconId == "germany_jagdpantherii")
            {
                //0.9.12 replay tank name changed to g71_jagdpantherii
                tankDescription = tankDescriptions[10045];
            }
            else if (iconId == "germany_pz_ii_ausfg")
            {
                //0.9.12 replay tank name changed to g82_pz_ii_ausfg
                tankDescription = tankDescriptions[10051];
            }
            else if (iconId == "germany_vk2001db")
            {
                //0.9.12 replay tank name changed to g86_vk2001db
                tankDescription = tankDescriptions[10053];
            }
            else if (iconId == "germany_gw_mk_vie")
            {
                //0.9.12 replay tank name changed to g93_gw_mk_vie
                tankDescription = tankDescriptions[10059];
            }
            else if (iconId == "germany_s35_captured")
            {
                //0.9.12 replay tank name changed to g34_s35_captured
                tankDescription = tankDescriptions[10203];
            }
            else if (iconId == "germany_jagdtiger_sdkfz_185")
            {
                //0.9.12 replay tank name changed to g65_jagdtiger_sdkfz_185
                tankDescription = tankDescriptions[10216];
            }
            else if (iconId == "germany_pzvib_tiger_ii_training")
            {
                //0.9.12 replay tank name changed to g16_pzvib_tiger_ii_training
                tankDescription = tankDescriptions[10227];
            }
            else if (iconId == "germany_jagdtiger_sdkfz_185_igr")
            {
                //0.9.12 replay tank name changed to g65_jagdtiger_sdkfz_185_igr
                tankDescription = tankDescriptions[10151];
            }
            else if (iconId == "germany_pzvib_tiger_ii_igr")
            {
                //0.9.12 replay tank name changed to g16_pzvib_tiger_ii_igr
                tankDescription = tankDescriptions[10153];
            }
            else if (iconId == "uk_gb25_loyd_carrier")
            {
                //0.9.12 replay tank name changed to gb25_loyd_gun_carriage
                tankDescription = tankDescriptions[50041];
            }
            else if (iconId == "uk_gb70_fv4202_105")
            {
                //0.9.12 replay tank name changed to gb86_centurion_action_x
                tankDescription = tankDescriptions[50028];
            }
            else if (iconId == "usa_m3_stuart")
            {
                //0.9.12 replay tank name changed to a03_m3_stuart
                tankDescription = tankDescriptions[20001];
            }
            else if (iconId == "usa_t34_hvy")
            {
                //0.9.12 replay tank name changed to a13_t34_hvy
                tankDescription = tankDescriptions[20011];
            }
            else if (iconId == "usa_m2_med")
            {
                //0.9.12 replay tank name changed to a25_m2_med
                tankDescription = tankDescriptions[20019];
            }
            else if (iconId == "usa_m7_med")
            {
                //0.9.12 replay tank name changed to a23_m7_med
                tankDescription = tankDescriptions[20021];
            }
            else if (iconId == "usa_t2_med")
            {
                //0.9.12 replay tank name changed to a24_t2_med
                tankDescription = tankDescriptions[20022];
            }
            else if (iconId == "usa_t82")
            {
                //0.9.12 replay tank name changed to a109_t56_gmc
                tankDescription = tankDescriptions[20025];
            }
            else if (iconId == "usa_t18")
            {
                //0.9.12 replay tank name changed to a46_t3
                tankDescription = tankDescriptions[20024];
            }
            else if (iconId == "usa_t57")
            {
                //0.9.12 replay tank name changed to a107_t1_hmc
                tankDescription = tankDescriptions[20008];
            }
            else if (iconId == "usa_t34_hvy_igr")
            {
                //0.9.12 replay tank name changed to a13_t34_hvy_igr
                tankDescription = tankDescriptions[20153];
            }
            else if (iconId == "ussr_object_212")
            {
                //0.9.12 replay tank name changed to r51_object_212
                tankDescription = tankDescriptions[33];
            }
            else if (iconId == "ussr_is8")
            {
                //0.9.12 replay tank name changed to r81_is8
                tankDescription = tankDescriptions[45];
            }
            else if (iconId == "ussr_kv1")
            {
                //0.9.12 replay tank name changed to r80_kv1
                tankDescription = tankDescriptions[46];
            }
            else if (iconId == "ussr_kv_220")
            {
                //0.9.12 replay tank name changed to r38_kv_220
                tankDescription = tankDescriptions[200];
            }
            else if (iconId == "ussr_kv_220_action")
            {
                //0.9.12 replay tank name changed to r38_kv_220_action
                tankDescription = tankDescriptions[211];
            }
            else if (iconId == "france_amx_ac_mle1948")
            {
                //0.9.13 replay tank name changed to f36_amx_ac_mle1948
                tankDescription = tankDescriptions[40047];
            }
            else if (iconId == "france_amx_ac_mle1948_igr")
            {
                //0.9.13 replay tank name changed to f36_amx_ac_mle1948_igr
                tankDescription = tankDescriptions[40151];
            }
            else if (iconId == "germany_pzii_luchs")
            {
                //0.9.13 replay tank name changed to g25_pzii_luchs
                tankDescription = tankDescriptions[10024];
            }
            else if (iconId == "japan_ha_go")
            {
                //0.9.13 replay tank name changed to j03_ha_go
                tankDescription = tankDescriptions[60003];
            }
            else if (iconId == "japan_ke_ni")
            {
                //0.9.13 replay tank name changed to j04_ke_ni
                tankDescription = tankDescriptions[60009];
            }
            else if (iconId == "usa_t30")
            {
                //0.9.13 replay tank name changed to a14_t30
                tankDescription = tankDescriptions[20010];
            }
            else if (iconId == "usa_m7_priest")
            {
                //0.9.13 replay tank name changed to a16_m7_priest
                tankDescription = tankDescriptions[20014];
            }
            else if (iconId == "usa_t29")
            {
                //0.9.13 replay tank name changed to a11_t29
                tankDescription = tankDescriptions[20015];
            }
            else if (iconId == "usa_t54e1")
            {
                //0.9.13 replay tank name changed to a89_t54e1
                tankDescription = tankDescriptions[20060];
            }
            else if (iconId == "usa_t95_e6")
            {
                //0.9.13 replay tank name changed to a95_t95_e6
                tankDescription = tankDescriptions[20218];
            }
            else if (iconId == "usa_t29_igr")
            {
                //0.9.13 replay tank name changed to a11_t29_igr
                tankDescription = tankDescriptions[20157];
            }
            else if (iconId == "ussr_su_152")
            {
                //0.9.13 replay tank name changed to r18_su_152
                tankDescription = tankDescriptions[9];
            }
            else if (iconId == "ussr_su_18")
            {
                //0.9.13 replay tank name changed to r16_su_18
                tankDescription = tankDescriptions[15];
            }
            else if (iconId == "ussr_kv_3")
            {
                //0.9.13 replay tank name changed to r39_kv_3
                tankDescription = tankDescriptions[23];
            }
            else if (iconId == "ussr_kv_5")
            {
                //0.9.13 replay tank name changed to r54_kv_5
                tankDescription = tankDescriptions[208];
            }
            else if (iconId == "ussr_ltp")
            {
                //0.9.13 replay tank name changed to r86_ltp
                tankDescription = tankDescriptions[221];
            }
            else if (iconId == "ussr_su_152_igr")
            {
                //0.9.13 replay tank name changed to r18_su_152_igr
                tankDescription = tankDescriptions[156];
            }
            else if (iconId == "ussr_kv_5_igr")
            {
                //0.9.13 replay tank name changed to r54_kv_5_igr
                tankDescription = tankDescriptions[157];
            }
            else if (iconId == "france__105_lefh18b2")
            {
                //0.9.14 replay tank name changed to f28_105_lefh18b2
                tankDescription = tankDescriptions[40008];
            }
            else if (iconId == "france_amx_50fosh_155")
            {
                //0.9.14 replay tank name changed to f64_amx_50fosh_155
                tankDescription = tankDescriptions[40054];
            }
            else if (iconId == "france__105_lefh18b2_igr")
            {
                //0.9.14 replay tank name changed to f28_105_lefh18b2_igr
                tankDescription = tankDescriptions[40154];
            }
            else if (iconId == "germany_vk3001h")
            {
                //0.9.14 replay tank name changed to g13_vk3001h
                tankDescription = tankDescriptions[10010];
            }
            else if (iconId == "germany_g_tiger")
            {
                //0.9.14 replay tank name changed to g45_g_tiger
                tankDescription = tankDescriptions[10034];
            }
            else if (iconId == "germany_nashorn")
            {
                //0.9.14 replay tank name changed to g40_nashorn
                tankDescription = tankDescriptions[10046];
            }
            else if (iconId == "germany_t_25")
            {
                //0.9.14 replay tank name changed to g46_t_25
                tankDescription = tankDescriptions[10213];
            }
            else if (iconId == "germany_e_25")
            {
                //0.9.14 replay tank name changed to g48_e_25
                tankDescription = tankDescriptions[10217];
            }
            else if (iconId == "germany_vk7201")
            {
                //0.9.14 replay tank name changed to g92_vk7201
                tankDescription = tankDescriptions[10229];
            }
            else if (iconId == "germany_e_25_igr")
            {
                //0.9.14 replay tank name changed to g48_e_25_igr
                tankDescription = tankDescriptions[10156];
            }
            else if (iconId == "japan_chi_ni")
            {
                //0.9.14 replay tank name changed to j15_chi_ni
                tankDescription = tankDescriptions[60001];
            }
            else if (iconId == "japan_chi_nu")
            {
                //0.9.14 replay tank name changed to j08_chi_nu
                tankDescription = tankDescriptions[60005];
            }
            else if (iconId == "japan_chi_ha")
            {
                //0.9.14 replay tank name changed to j07_chi_ha
                tankDescription = tankDescriptions[60008];
            }
            else if (iconId == "japan_ke_ho")
            {
                //0.9.14 replay tank name changed to j06_ke_ho
                tankDescription = tankDescriptions[60011];
            }
            else if (iconId == "usa_m4_sherman")
            {
                //0.9.14 replay tank name changed to a05_m4_sherman
                tankDescription = tankDescriptions[20004];
            }
            else if (iconId == "usa_t20")
            {
                //0.9.14 replay tank name changed to a07_t20
                tankDescription = tankDescriptions[20006];
            }
            else if (iconId == "usa_t1_hvy")
            {
                //0.9.14 replay tank name changed to a09_t1_hvy
                tankDescription = tankDescriptions[20013];
            }
            else if (iconId == "usa_t40")
            {
                //0.9.14 replay tank name changed to a29_t40
                tankDescription = tankDescriptions[20030];
            }
            else if (iconId == "usa_t28_prototype")
            {
                //0.9.14 replay tank name changed to a68_t28_prototype
                tankDescription = tankDescriptions[20044];
            }
            else if (iconId == "usa_m53_55")
            {
                //0.9.14 replay tank name changed to a88_m53_55
                tankDescription = tankDescriptions[20063];
            }
            else if (iconId == "usa_m44")
            {
                //0.9.14 replay tank name changed to a87_m44
                tankDescription = tankDescriptions[20064];
            }
            else if (iconId == "usa_m4_sherman_igr")
            {
                //0.9.14 replay tank name changed to a05_m4_sherman_igr
                tankDescription = tankDescriptions[20158];
            }
            else if (iconId == "ussr_is")
            {
                //0.9.14 replay tank name changed to r01_is
                tankDescription = tankDescriptions[2];
            }
            else if (iconId == "ussr_a_20")
            {
                //0.9.14 replay tank name changed to r12_a_20
                tankDescription = tankDescriptions[8];
            }
            else if (iconId == "ussr_su_100")
            {
                //0.9.14 replay tank name changed to r17_su_100
                tankDescription = tankDescriptions[14];
            }
            else if (iconId == "ussr_ms_1")
            {
                //0.9.14 replay tank name changed to r11_ms_1
                tankDescription = tankDescriptions[13];
            }
            else if (iconId == "ussr_object252")
            {
                //0.9.14 replay tank name changed to r61_object252
                tankDescription = tankDescriptions[36];
            }
            else if (iconId == "ussr_ms_1_bot")
            {
                //0.9.14 replay tank name changed to r11_ms_1_bot
                tankDescription = tankDescriptions[160];
            }
            else if (iconId == "france_amx_13_90")
            {
                //0.9.15 replay tank name changed to f17_amx_13_90
                tankDescription = tankDescriptions[40019];
            }
            else if (iconId == "france_amx_13_90_igr")
            {
                //0.9.15 replay tank name changed to f17_amx_13_90_igr
                tankDescription = tankDescriptions[40152];
            }
            else if (iconId == "germany_stug_40_ausfg")
            {
                //0.9.15 replay tank name changed to g05_stug_40_ausfg
                tankDescription = tankDescriptions[10004];
            }
            else if (iconId == "germany_pz38t")
            {
                //0.9.15 replay tank name changed to g08_pz38t
                tankDescription = tankDescriptions[10013];
            }
            else if (iconId == "germany_vk3001p")
            {
                //0.9.15 replay tank name changed to g27_vk3001p
                tankDescription = tankDescriptions[10028];
            }
            else if (iconId == "germany_stug_40_ausfg_igr")
            {
                //0.9.15 replay tank name changed to g05_stug_40_ausfg_igr
                tankDescription = tankDescriptions[10155];
            }
            else if (iconId == "japan_chi_he")
            {
                //0.9.15 replay tank name changed to j09_chi_he
                tankDescription = tankDescriptions[60006];
            }
            else if (iconId == "japan_chi_nu_kai")
            {
                //0.9.15 replay tank name changed to j12_chi_nu_kai
                tankDescription = tankDescriptions[60201];
            }
            else if (iconId == "usa_t1_cunningham")
            {
                //0.9.15 replay tank name changed to a01_t1_cunningham
                tankDescription = tankDescriptions[20002];
            }
            else if (iconId == "usa_t67")
            {
                //0.9.15 replay tank name changed to a58_t67
                tankDescription = tankDescriptions[20041];
            }
            else if (iconId == "usa_m60")
            {
                //0.9.15 replay tank name changed to a92_m60
                tankDescription = tankDescriptions[20062];
            }
            else if (iconId == "usa_t67_igr")
            {
                //0.9.15 replay tank name changed to a58_t67_igr
                tankDescription = tankDescriptions[20155];
            }
            else if (iconId == "usa_t1_cunningham_bot")
            {
                //0.9.15 replay tank name changed to a01_t1_cunningham_bot
                tankDescription = tankDescriptions[20160];
            }
            else if (iconId == "ussr_bt_7")
            {
                //0.9.15 replay tank name changed to r03_bt_7
                tankDescription = tankDescriptions[3];
            }
            else if (iconId == "ussr_t_34_85")
            {
                //0.9.15 replay tank name changed to r07_t_34_85
                tankDescription = tankDescriptions[10];
            }
            else if (iconId == "ussr_t_46")
            {
                //0.9.15 replay tank name changed to r22_t_46
                tankDescription = tankDescriptions[12];
            }
            else if (iconId == "ussr_t_43")
            {
                //0.9.15 replay tank name changed to r23_t_43
                tankDescription = tankDescriptions[26];
            }
            else if (iconId == "ussr_gaz_74b")
            {
                //0.9.15 replay tank name changed to r25_gaz_74b
                tankDescription = tankDescriptions[27];
            }
            else if (iconId == "ussr_su122_54")
            {
                //0.9.15 replay tank name changed to r75_su122_54
                tankDescription = tankDescriptions[47];
            }
            else if (iconId == "ussr_t80")
            {
                //0.9.15 replay tank name changed to r44_t80
                tankDescription = tankDescriptions[62];
            }
            else if (iconId == "ussr_object_430")
            {
                //0.9.15 replay tank name changed to r96_object_430
                tankDescription = tankDescriptions[67];
            }
            else if (iconId == "ussr_t_127")
            {
                //0.9.15 replay tank name changed to r56_t_127
                tankDescription = tankDescriptions[209];
            }
            else if (iconId == "ussr_t_34_85_training")
            {
                //0.9.15 replay tank name changed to r07_t_34_85_training
                tankDescription = tankDescriptions[220];
            }
            else if (iconId == "france_d2")
            {
                //0.9.15.1 replay tank name changed to f03_d2
                tankDescription = tankDescriptions[40001];
            }
            else if (iconId == "france_renaultft")
            {
                //0.9.15.1 replay tank name changed to f01_renaultft
                tankDescription = tankDescriptions[40002];
            }
            else if (iconId == "france_renaultbs")
            {
                //0.9.15.1 replay tank name changed to f20_renaultbs
                tankDescription = tankDescriptions[40003];
            }
            else if (iconId == "france_b1")
            {
                //0.9.15.1 replay tank name changed to f04_b1
                tankDescription = tankDescriptions[40004];
            }
            else if (iconId == "france_hotchkiss_h35")
            {
                //0.9.15.1 replay tank name changed to f12_hotchkiss_h35
                tankDescription = tankDescriptions[40005];
            }
            else if (iconId == "france_d1")
            {
                //0.9.15.1 replay tank name changed to f02_d1
                tankDescription = tankDescriptions[40006];
            }
            else if (iconId == "france_fcm_36pak40")
            {
                //0.9.15.1 replay tank name changed to f27_fcm_36pak40
                tankDescription = tankDescriptions[40009];
            }
            else if (iconId == "france_amx40")
            {
                //0.9.15.1 replay tank name changed to f14_amx40
                tankDescription = tankDescriptions[40011];
            }
            else if (iconId == "france_lorraine39_l_am")
            {
                //0.9.15.1 replay tank name changed to f21_lorraine39_l_am
                tankDescription = tankDescriptions[40013];
            }
            else if (iconId == "france_bat_chatillon25t")
            {
                //0.9.15.1 replay tank name changed to f18_bat_chatillon25t
                tankDescription = tankDescriptions[40014];
            }
            else if (iconId == "france_amx_105am")
            {
                //0.9.15.1 replay tank name changed to f22_amx_105am
                tankDescription = tankDescriptions[40016];
            }
            else if (iconId == "france_amx_13f3am")
            {
                //0.9.15.1 replay tank name changed to f23_amx_13f3am
                tankDescription = tankDescriptions[40018];
            }
            else if (iconId == "france_amx_13_75")
            {
                //0.9.15.1 replay tank name changed to f16_amx_13_75
                tankDescription = tankDescriptions[40020];
            }
            else if (iconId == "france_f75_char_de_25t")
            {
                //0.9.15.1 replay tank name changed to f19_lorraine40t
                tankDescription = tankDescriptions[40244];
            }
            else if (iconId == "france_amx38")
            {
                //0.9.15.1 replay tank name changed to f13_amx38
                tankDescription = tankDescriptions[40023];
            }
            else if (iconId == "france_bdr_g1b")
            {
                //0.9.15.1 replay tank name changed to f05_bdr_g1b
                tankDescription = tankDescriptions[40026];
            }
            else if (iconId == "france_lorraine155_50")
            {
                //0.9.15.1 replay tank name changed to f24_lorraine155_50
                tankDescription = tankDescriptions[40028];
            }
            else if (iconId == "france_lorraine155_51")
            {
                //0.9.15.1 replay tank name changed to f25_lorraine155_51
                tankDescription = tankDescriptions[40029];
            }
            else if (iconId == "france_renaultft_ac")
            {
                //0.9.15.1 replay tank name changed to f30_renaultft_ac
                tankDescription = tankDescriptions[40030];
            }
            else if (iconId == "france_renaultue57")
            {
                //0.9.15.1 replay tank name changed to f52_renaultue57
                tankDescription = tankDescriptions[40032];
            }
            else if (iconId == "france_somua_sau_40")
            {
                //0.9.15.1 replay tank name changed to f32_somua_sau_40
                tankDescription = tankDescriptions[40038];
            }
            else if (iconId == "france_s_35ca")
            {
                //0.9.15.1 replay tank name changed to f33_s_35ca
                tankDescription = tankDescriptions[40039];
            }
            else if (iconId == "france_amx50_foch")
            {
                //0.9.15.1 replay tank name changed to f37_amx50_foch
                tankDescription = tankDescriptions[40043];
            }
            else if (iconId == "france_arl_v39")
            {
                //0.9.15.1 replay tank name changed to f34_arl_v39
                tankDescription = tankDescriptions[40045];
            }
            else if (iconId == "france_bat_chatillon155_58")
            {
                //0.9.15.1 replay tank name changed to f38_bat_chatillon155_58
                tankDescription = tankDescriptions[40046];
            }
            else if (iconId == "france_elc_amx")
            {
                //0.9.15.1 replay tank name changed to f62_elc_amx
                tankDescription = tankDescriptions[40055];
            }
            else if (iconId == "france_bat_chatillon155_55")
            {
                //0.9.15.1 replay tank name changed to f67_bat_chatillon155_55
                tankDescription = tankDescriptions[40056];
            }
            else if (iconId == "france_amx_ob_am105")
            {
                //0.9.15.1 replay tank name changed to f66_amx_ob_am105
                tankDescription = tankDescriptions[40057];
            }
            else if (iconId == "france_lorraine40t")
            {
                //0.9.15.1 replay tank name changed to f75_char_de_25t
                tankDescription = tankDescriptions[40022];
            }
            else if (iconId == "france_s_35ca_igr")
            {
                //0.9.15.1 replay tank name changed to f33_s_35ca_igr
                tankDescription = tankDescriptions[40155];
            }
            else if (iconId == "france_elc_amx_igr")
            {
                //0.9.15.1 replay tank name changed to f62_elc_amx_igr
                tankDescription = tankDescriptions[40156];
            }
            else if (iconId == "france_bat_chatillon25t_igr")
            {
                //0.9.15.1 replay tank name changed to f18_bat_chatillon25t_igr
                tankDescription = tankDescriptions[40157];
            }
            else if (iconId == "france_renaultft_bot")
            {
                //0.9.15.1 replay tank name changed to f01_renaultft_bot
                tankDescription = tankDescriptions[40160];
            }
            else if (iconId == "germany_pziv")
            {
                //0.9.15.1 replay tank name changed to g79_pz_iv_ausfgh
                tankDescription = tankDescriptions[10000];
            }
            else if (iconId == "germany_pzvi")
            {
                //0.9.15.1 replay tank name changed to g04_pzvi_tiger_i
                tankDescription = tankDescriptions[10002];
            }
            else if (iconId == "germany_pzv")
            {
                //0.9.15.1 replay tank name changed to g03_pzv_panther
                tankDescription = tankDescriptions[10005];
            }
            else if (iconId == "germany_hetzer")
            {
                //0.9.15.1 replay tank name changed to g09_hetzer
                tankDescription = tankDescriptions[10007];
            }
            else if (iconId == "germany_ltraktor")
            {
                //0.9.15.1 replay tank name changed to g12_ltraktor
                tankDescription = tankDescriptions[10012];
            }
            else if (iconId == "germany_jagdpanther")
            {
                //0.9.15.1 replay tank name changed to g18_jagdpanther
                tankDescription = tankDescriptions[10015];
            }
            else if (iconId == "germany_vk3002db")
            {
                //0.9.15.1 replay tank name changed to g24_vk3002db
                tankDescription = tankDescriptions[10016];
            }
            else if (iconId == "germany_pziii_ausfj")
            {
                //0.9.15.1 replay tank name changed to g10_pziii_ausfj
                tankDescription = tankDescriptions[10017];
            }
            else if (iconId == "germany_pziii_a")
            {
                //0.9.15.1 replay tank name changed to g14_pziii_a
                tankDescription = tankDescriptions[10019];
            }
            else if (iconId == "germany_pziii_iv")
            {
                //0.9.15.1 replay tank name changed to g28_pziii_iv
                tankDescription = tankDescriptions[10025];
            }
            else if (iconId == "germany_maus")
            {
                //0.9.15.1 replay tank name changed to g42_maus
                tankDescription = tankDescriptions[10027];
            }
            else if (iconId == "germany_vk4502p")
            {
                //0.9.15.1 replay tank name changed to g58_vk4502p
                tankDescription = tankDescriptions[10029];
            }
            else if (iconId == "germany_ferdinand")
            {
                //0.9.15.1 replay tank name changed to g37_ferdinand
                tankDescription = tankDescriptions[10030];
            }
            else if (iconId == "germany_panther_ii")
            {
                //0.9.15.1 replay tank name changed to g64_panther_ii
                tankDescription = tankDescriptions[10033];
            }
            else if (iconId == "germany_g_panther")
            {
                //0.9.15.1 replay tank name changed to g49_g_panther
                tankDescription = tankDescriptions[10035];
            }
            else if (iconId == "germany_g_e")
            {
                //0.9.15.1 replay tank name changed to g61_g_e
                tankDescription = tankDescriptions[10036];
            }
            else if (iconId == "germany_e_100")
            {
                //0.9.15.1 replay tank name changed to g56_e_100
                tankDescription = tankDescriptions[10037];
            }
            else if (iconId == "germany_e_75")
            {
                //0.9.15.1 replay tank name changed to g55_e_75
                tankDescription = tankDescriptions[10038];
            }
            else if (iconId == "germany_vk2801")
            {
                //0.9.15.1 replay tank name changed to g66_vk2801
                tankDescription = tankDescriptions[10039];
            }
            else if (iconId == "germany_e_50")
            {
                //0.9.15.1 replay tank name changed to g54_e_50
                tankDescription = tankDescriptions[10040];
            }
            else if (iconId == "germany_vk4502a")
            {
                //0.9.15.1 replay tank name changed to g67_vk4502a
                tankDescription = tankDescriptions[10041];
            }
            else if (iconId == "germany_pzvi_tiger_p")
            {
                //0.9.15.1 replay tank name changed to g57_pzvi_tiger_p
                tankDescription = tankDescriptions[10042];
            }
            else if (iconId == "germany_sturer_emil")
            {
                //0.9.15.1 replay tank name changed to g43_sturer_emil
                tankDescription = tankDescriptions[10043];
            }
            else if (iconId == "germany_jagdpz_e100")
            {
                //0.9.15.1 replay tank name changed to g72_jagdpz_e100
                tankDescription = tankDescriptions[10047];
            }
            else if (iconId == "germany_e50_ausf_m")
            {
                //0.9.15.1 replay tank name changed to g73_e50_ausf_m
                tankDescription = tankDescriptions[10048];
            }
            else if (iconId == "germany_pzi_ausf_c")
            {
                //0.9.15.1 replay tank name changed to g63_pzi_ausf_c
                tankDescription = tankDescriptions[10049];
            }
            else if (iconId == "germany_pzi")
            {
                //0.9.15.1 replay tank name changed to g53_pzi
                tankDescription = tankDescriptions[10050];
            }
            else if (iconId == "germany_dw_ii")
            {
                //0.9.15.1 replay tank name changed to g90_dw_ii
                tankDescription = tankDescriptions[10052];
            }
            else if (iconId == "germany_indien_panzer")
            {
                //0.9.15.1 replay tank name changed to g88_indien_panzer
                tankDescription = tankDescriptions[10054];
            }
            else if (iconId == "germany_vk3002db_v1")
            {
                //0.9.15.1 replay tank name changed to g87_vk3002db_v1
                tankDescription = tankDescriptions[10055];
            }
            else if (iconId == "germany_auf_panther")
            {
                //0.9.15.1 replay tank name changed to g85_auf_panther
                tankDescription = tankDescriptions[10056];
            }
            else if (iconId == "germany_gw_tiger_p")
            {
                //0.9.15.1 replay tank name changed to g94_gw_tiger_p
                tankDescription = tankDescriptions[10060];
            }
            else if (iconId == "germany_pz_sfl_ivb")
            {
                //0.9.15.1 replay tank name changed to g95_pz_sfl_ivb
                tankDescription = tankDescriptions[10061];
            }
            else if (iconId == "germany_vk3002m")
            {
                //0.9.15.1 replay tank name changed to g96_vk3002m
                tankDescription = tankDescriptions[10062];
            }
            else if (iconId == "germany_pz_sfl_ivc")
            {
                //0.9.15.1 replay tank name changed to g76_pz_sfl_ivc
                tankDescription = tankDescriptions[10063];
            }
            else if (iconId == "germany_waffentrager_iv")
            {
                //0.9.15.1 replay tank name changed to g97_waffentrager_iv
                tankDescription = tankDescriptions[10064];
            }
            else if (iconId == "germany_rhb_waffentrager")
            {
                //0.9.15.1 replay tank name changed to g99_rhb_waffentrager
                tankDescription = tankDescriptions[10065];
            }
            else if (iconId == "germany_waffentrager_e100")
            {
                //0.9.15.1 replay tank name changed to g98_waffentrager_e100
                tankDescription = tankDescriptions[10066];
            }
            else if (iconId == "germany_pz_iv_ausfa")
            {
                //0.9.15.1 replay tank name changed to g83_pz_iv_ausfa
                tankDescription = tankDescriptions[10067];
            }
            else if (iconId == "germany_pz_iv_ausfd")
            {
                //0.9.15.1 replay tank name changed to g80_pz_iv_ausfd
                tankDescription = tankDescriptions[10068];
            }
            else if (iconId == "germany_pz_iv_ausfh")
            {
                //0.9.15.1 replay tank name changed to g81_pz_iv_ausfh
                tankDescription = tankDescriptions[10071];
            }
            else if (iconId == "germany_pzv_pziv")
            {
                //0.9.15.1 replay tank name changed to g32_pzv_pziv
                tankDescription = tankDescriptions[10201];
            }
            else if (iconId == "germany_b_1bis_captured")
            {
                //0.9.15.1 replay tank name changed to g35_b_1bis_captured
                tankDescription = tankDescriptions[10204];
            }
            else if (iconId == "germany_h39_captured")
            {
                //0.9.15.1 replay tank name changed to g33_h39_captured
                tankDescription = tankDescriptions[10205];
            }
            else if (iconId == "germany_pzv_pziv_ausf_alfa")
            {
                //0.9.15.1 replay tank name changed to g32_pzv_pziv_ausf_alfa
                tankDescription = tankDescriptions[10211];
            }
            else if (iconId == "germany_lowe")
            {
                //0.9.15.1 replay tank name changed to g51_lowe
                tankDescription = tankDescriptions[10212];
            }
            else if (iconId == "germany_t_15")
            {
                //0.9.15.1 replay tank name changed to g50_t_15
                tankDescription = tankDescriptions[10214];
            }
            else if (iconId == "germany_pziv_hydro")
            {
                //0.9.15.1 replay tank name changed to g70_pziv_hydro
                tankDescription = tankDescriptions[10215];
            }
            else if (iconId == "germany_dickermax")
            {
                //0.9.15.1 replay tank name changed to g41_dickermax
                tankDescription = tankDescriptions[10223];
            }
            else if (iconId == "germany_pziv_schmalturm")
            {
                //0.9.15.1 replay tank name changed to g77_pziv_schmalturm
                tankDescription = tankDescriptions[10224];
            }
            else if (iconId == "germany_pziii_training")
            {
                //0.9.15.1 replay tank name changed to g10_pziii_ausfj_training
                tankDescription = tankDescriptions[10226];
            }
            else if (iconId == "germany_pzv_training")
            {
                //0.9.15.1 replay tank name changed to g03_pzv_panther_training
                tankDescription = tankDescriptions[10228];
            }
            else if (iconId == "germany_g58_vk4502p")
            {
                //0.9.15.1 replay tank name changed to g58_vk4502p7
                tankDescription = tankDescriptions[10243];
            }
            else if (iconId == "germany_panther_ii_igr")
            {
                //0.9.15.1 replay tank name changed to g64_panther_ii_igr
                tankDescription = tankDescriptions[10152];
            }
            else if (iconId == "germany_pzvi_igr")
            {
                //0.9.15.1 replay tank name changed to g04_pzvi_tiger_i_igr
                tankDescription = tankDescriptions[10154];
            }
            else if (iconId == "germany_pzv_igr")
            {
                //0.9.15.1 replay tank name changed to g03_pzv_panther_igr
                tankDescription = tankDescriptions[10157];
            }
            else if (iconId == "germany_ferdinand_igr")
            {
                //0.9.15.1 replay tank name changed to g37_ferdinand_igr
                tankDescription = tankDescriptions[10158];
            }
            else if (iconId == "germany_maus_igr")
            {
                //0.9.15.1 replay tank name changed to g42_maus_igr
                tankDescription = tankDescriptions[10159];
            }
            else if (iconId == "germany_ltraktor_bot")
            {
                //0.9.15.1 replay tank name changed to g12_ltraktor_bot
                tankDescription = tankDescriptions[10160];
            }
            else if (iconId == "japan_nc27")
            {
                //0.9.15.1 replay tank name changed to j01_nc27
                tankDescription = tankDescriptions[60002];
            }
            else if (iconId == "japan_chi_ri")
            {
                //0.9.15.1 replay tank name changed to j11_chi_ri
                tankDescription = tankDescriptions[60004];
            }
            else if (iconId == "japan_chi_to")
            {
                //0.9.15.1 replay tank name changed to j10_chi_to
                tankDescription = tankDescriptions[60007];
            }
            else if (iconId == "japan_sta_1")
            {
                //0.9.15.1 replay tank name changed to j13_sta_1
                tankDescription = tankDescriptions[60010];
            }
            else if (iconId == "japan_te_ke")
            {
                //0.9.15.1 replay tank name changed to j02_te_ke
                tankDescription = tankDescriptions[60012];
            }
            else if (iconId == "japan_type_61")
            {
                //0.9.15.1 replay tank name changed to j14_type_61
                tankDescription = tankDescriptions[60013];
            }
            else if (iconId == "japan_st_b1")
            {
                //0.9.15.1 replay tank name changed to j16_st_b1
                tankDescription = tankDescriptions[60014];
            }
            else if (iconId == "japan_ke_ni_b")
            {
                //0.9.15.1 replay tank name changed to j05_ke_ni_b
                tankDescription = tankDescriptions[60202];
            }
            else if (iconId == "japan_sta_1_igr")
            {
                //0.9.15.1 replay tank name changed to j13_sta_1_igr
                tankDescription = tankDescriptions[60151];
            }
            else if (iconId == "japan_chi_ri_igr")
            {
                //0.9.15.1 replay tank name changed to j11_chi_ri_igr
                tankDescription = tankDescriptions[60152];
            }
            else if (iconId == "japan_st_b1_igr")
            {
                //0.9.15.1 replay tank name changed to j16_st_b1_igr
                tankDescription = tankDescriptions[60153];
            }
            else if (iconId == "japan_nc27_bot")
            {
                //0.9.15.1 replay tank name changed to j01_nc27_bot
                tankDescription = tankDescriptions[60160];
            }
            else if (iconId == "usa_t14")
            {
                //0.9.15.1 replay tank name changed to a21_t14
                tankDescription = tankDescriptions[20000];
            }
            else if (iconId == "usa_m6")
            {
                //0.9.15.1 replay tank name changed to a10_m6
                tankDescription = tankDescriptions[20003];
            }
            else if (iconId == "usa_m4a3e8_sherman")
            {
                //0.9.15.1 replay tank name changed to a06_m4a3e8_sherman
                tankDescription = tankDescriptions[20005];
            }
            else if (iconId == "usa_t57")
            {
                //0.9.15.1 replay tank name changed to a15_t57
                tankDescription = tankDescriptions[20075];
            }
            else if (iconId == "usa_t23")
            {
                //0.9.15.1 replay tank name changed to a08_t23
                tankDescription = tankDescriptions[20009];
            }
            else if (iconId == "usa_m3_grant")
            {
                //0.9.15.1 replay tank name changed to a04_m3_grant
                tankDescription = tankDescriptions[20012];
            }
            else if (iconId == "usa_m37")
            {
                //0.9.15.1 replay tank name changed to a17_m37
                tankDescription = tankDescriptions[20018];
            }
            else if (iconId == "usa_m5_stuart")
            {
                //0.9.15.1 replay tank name changed to a22_m5_stuart
                tankDescription = tankDescriptions[20020];
            }
            else if (iconId == "usa_t18")
            {
                //0.9.15.1 replay tank name changed to a26_t18
                tankDescription = tankDescriptions[20074];
            }
            else if (iconId == "usa_m36_slagger")
            {
                //0.9.15.1 replay tank name changed to a31_m36_slagger
                tankDescription = tankDescriptions[20028];
            }
            else if (iconId == "usa_m40m43")
            {
                //0.9.15.1 replay tank name changed to a37_m40m43
                tankDescription = tankDescriptions[20029];
            }
            else if (iconId == "usa_m12")
            {
                //0.9.15.1 replay tank name changed to a32_m12
                tankDescription = tankDescriptions[20031];
            }
            else if (iconId == "usa_t28")
            {
                //0.9.15.1 replay tank name changed to a39_t28
                tankDescription = tankDescriptions[20032];
            }
            else if (iconId == "usa_t92")
            {
                //0.9.15.1 replay tank name changed to a38_t92
                tankDescription = tankDescriptions[20033];
            }
            else if (iconId == "usa_t95")
            {
                //0.9.15.1 replay tank name changed to a40_t95
                tankDescription = tankDescriptions[20034];
            }
            else if (iconId == "usa_t25_at")
            {
                //0.9.15.1 replay tank name changed to a64_t25_at
                tankDescription = tankDescriptions[20036];
            }
            else if (iconId == "usa_m103")
            {
                //0.9.15.1 replay tank name changed to a66_m103
                tankDescription = tankDescriptions[20037];
            }
            else if (iconId == "usa_m24_chaffee")
            {
                //0.9.15.1 replay tank name changed to a34_m24_chaffee
                tankDescription = tankDescriptions[20038];
            }
            else if (iconId == "usa_m8a1")
            {
                //0.9.15.1 replay tank name changed to a57_m8a1
                tankDescription = tankDescriptions[20040];
            }
            else if (iconId == "usa_t25_2")
            {
                //0.9.15.1 replay tank name changed to a72_t25_2
                tankDescription = tankDescriptions[20043];
            }
            else if (iconId == "usa_m18_hellcat")
            {
                //0.9.15.1 replay tank name changed to a41_m18_hellcat
                tankDescription = tankDescriptions[20045];
            }
            else if (iconId == "usa_t69")
            {
                //0.9.15.1 replay tank name changed to a90_t69
                tankDescription = tankDescriptions[20057];
            }
            else if (iconId == "usa_t57_58")
            {
                //0.9.15.1 replay tank name changed to a67_t57_58
                tankDescription = tankDescriptions[20058];
            }
            else if (iconId == "usa_t21")
            {
                //0.9.15.1 replay tank name changed to a71_t21
                tankDescription = tankDescriptions[20059];
            }
            else if (iconId == "usa_m41_bulldog")
            {
                //0.9.15.1 replay tank name changed to a97_m41_bulldog
                tankDescription = tankDescriptions[20070];
            }
            else if (iconId == "usa_t49")
            {
                //0.9.15.1 replay tank name changed to a100_t49
                tankDescription = tankDescriptions[20071];
            }
            else if (iconId == "usa_t2_lt")
            {
                //0.9.15.1 replay tank name changed to a19_t2_lt
                tankDescription = tankDescriptions[20201];
            }
            else if (iconId == "usa_ram_ii")
            {
                //0.9.15.1 replay tank name changed to a62_ram_ii
                tankDescription = tankDescriptions[20202];
            }
            else if (iconId == "usa_mtls_1g14")
            {
                //0.9.15.1 replay tank name changed to a33_mtls_1g14
                tankDescription = tankDescriptions[20203];
            }
            else if (iconId == "usa_m4a2e4")
            {
                //0.9.15.1 replay tank name changed to a44_m4a2e4
                tankDescription = tankDescriptions[20204];
            }
            else if (iconId == "usa_m22_locust")
            {
                //0.9.15.1 replay tank name changed to a43_m22_locust
                tankDescription = tankDescriptions[20206];
            }
            else if (iconId == "usa_t1_e6")
            {
                //0.9.15.1 replay tank name changed to a74_t1_e6
                tankDescription = tankDescriptions[20209];
            }
            else if (iconId == "usa_t95_e2")
            {
                //0.9.15.1 replay tank name changed to a81_t95_e2
                tankDescription = tankDescriptions[20210];
            }
            else if (iconId == "usa_m4a3e8_sherman_training")
            {
                //0.9.15.1 replay tank name changed to a06_m4a3e8_sherman_training
                tankDescription = tankDescriptions[20212];
            }
            else if (iconId == "usa_t7_combat_car")
            {
                //0.9.15.1 replay tank name changed to a93_t7_combat_car
                tankDescription = tankDescriptions[20215];
            }
            else if (iconId == "usa_t69_igr")
            {
                //0.9.15.1 replay tank name changed to a90_t69_igr
                tankDescription = tankDescriptions[20152];
            }
            else if (iconId == "ussr_bt_2")
            {
                //0.9.15.1 replay tank name changed to r08_bt_2
                tankDescription = tankDescriptions[4];
            }
            else if (iconId == "ussr_kv")
            {
                //0.9.15.1 replay tank name changed to r05_kv
                tankDescription = tankDescriptions[5];
            }
            else if (iconId == "ussr_s_51")
            {
                //0.9.15.1 replay tank name changed to r15_s_51
                tankDescription = tankDescriptions[7];
            }
            else if (iconId == "ussr_kv_1s")
            {
                //0.9.15.1 replay tank name changed to r13_kv_1s
                tankDescription = tankDescriptions[73];
            }
            else if (iconId == "ussr_su_14")
            {
                //0.9.15.1 replay tank name changed to r27_su_14
                tankDescription = tankDescriptions[16];
            }
            else if (iconId == "ussr_t_44")
            {
                //0.9.15.1 replay tank name changed to r20_t_44
                tankDescription = tankDescriptions[17];
            }
            else if (iconId == "ussr_t_26")
            {
                //0.9.15.1 replay tank name changed to r09_t_26
                tankDescription = tankDescriptions[18];
            }
            else if (iconId == "ussr_su_5")
            {
                //0.9.15.1 replay tank name changed to r14_su_5
                tankDescription = tankDescriptions[19];
            }
            else if (iconId == "ussr_at_1")
            {
                //0.9.15.1 replay tank name changed to r10_at_1
                tankDescription = tankDescriptions[20];
            }
            else if (iconId == "ussr_su_8")
            {
                //0.9.15.1 replay tank name changed to r26_su_8
                tankDescription = tankDescriptions[22];
            }
            else if (iconId == "ussr_is_7")
            {
                //0.9.15.1 replay tank name changed to r45_is_7
                tankDescription = tankDescriptions[28];
            }
            else if (iconId == "ussr_isu_152")
            {
                //0.9.15.1 replay tank name changed to r47_isu_152
                tankDescription = tankDescriptions[29];
            }
            else if (iconId == "ussr_su_26")
            {
                //0.9.15.1 replay tank name changed to r66_su_26
                tankDescription = tankDescriptions[30];
            }
            else if (iconId == "ussr_t_54")
            {
                //0.9.15.1 replay tank name changed to r40_t_54
                tankDescription = tankDescriptions[31];
            }
            else if (iconId == "ussr_object_261")
            {
                //0.9.15.1 replay tank name changed to r52_object_261
                tankDescription = tankDescriptions[34];
            }
            else if (iconId == "ussr_kv_13")
            {
                //0.9.15.1 replay tank name changed to r46_kv_13
                tankDescription = tankDescriptions[35];
            }
            else if (iconId == "ussr_t_50")
            {
                //0.9.15.1 replay tank name changed to r41_t_50
                tankDescription = tankDescriptions[37];
            }
            else if (iconId == "ussr_t_50_2")
            {
                //0.9.15.1 replay tank name changed to r70_t_50_2
                tankDescription = tankDescriptions[38];
            }
            else if (iconId == "ussr_su_101")
            {
                //0.9.15.1 replay tank name changed to r58_su_101
                tankDescription = tankDescriptions[39];
            }
            else if (iconId == "ussr_su100m1")
            {
                //0.9.15.1 replay tank name changed to r74_su100m1
                tankDescription = tankDescriptions[40];
            }
            else if (iconId == "ussr_kv2")
            {
                //0.9.15.1 replay tank name changed to r77_kv2
                tankDescription = tankDescriptions[41];
            }
            else if (iconId == "ussr_st_i")
            {
                //0.9.15.1 replay tank name changed to r63_st_i
                tankDescription = tankDescriptions[42];
            }
            else if (iconId == "ussr_kv4")
            {
                //0.9.15.1 replay tank name changed to r73_kv4
                tankDescription = tankDescriptions[43];
            }
            else if (iconId == "ussr_t150")
            {
                //0.9.15.1 replay tank name changed to r72_t150
                tankDescription = tankDescriptions[44];
            }
            else if (iconId == "ussr_a43")
            {
                //0.9.15.1 replay tank name changed to r57_a43
                tankDescription = tankDescriptions[48];
            }
            else if (iconId == "ussr_a44")
            {
                //0.9.15.1 replay tank name changed to r59_a44
                tankDescription = tankDescriptions[49];
            }
            else if (iconId == "ussr_object416")
            {
                //0.9.15.1 replay tank name changed to r60_object416
                tankDescription = tankDescriptions[52];
            }
            else if (iconId == "ussr_object268")
            {
                //0.9.15.1 replay tank name changed to r88_object268
                tankDescription = tankDescriptions[53];
            }
            else if (iconId == "ussr_object263")
            {
                //0.9.15.1 replay tank name changed to r93_object263
                tankDescription = tankDescriptions[56];
            }
            else if (iconId == "ussr_t_70")
            {
                //0.9.15.1 replay tank name changed to r43_t_70
                tankDescription = tankDescriptions[59];
            }
            else if (iconId == "ussr_t_60")
            {
                //0.9.15.1 replay tank name changed to r42_t_60
                tankDescription = tankDescriptions[60];
            }
            else if (iconId == "ussr_object_907")
            {
                //0.9.15.1 replay tank name changed to r95_object_907
                tankDescription = tankDescriptions[61];
            }
            else if (iconId == "ussr_su14_1")
            {
                //0.9.15.1 replay tank name changed to r91_su14_1
                tankDescription = tankDescriptions[63];
            }
            else if (iconId == "ussr_mt25")
            {
                //0.9.15.1 replay tank name changed to r101_mt25
                tankDescription = tankDescriptions[65];
            }
            else if (iconId == "ussr_object_140")
            {
                //0.9.15.1 replay tank name changed to r97_object_140
                tankDescription = tankDescriptions[66];
            }
            else if (iconId == "ussr_matilda_ii_ll")
            {
                //0.9.15.1 replay tank name changed to r32_matilda_ii_ll
                tankDescription = tankDescriptions[201];
            }
            else if (iconId == "ussr_churchill_ll")
            {
                //0.9.15.1 replay tank name changed to r33_churchill_ll
                tankDescription = tankDescriptions[202];
            }
            else if (iconId == "ussr_bt_sv")
            {
                //0.9.15.1 replay tank name changed to r34_bt_sv
                tankDescription = tankDescriptions[204];
            }
            else if (iconId == "ussr_valentine_ll")
            {
                //0.9.15.1 replay tank name changed to r31_valentine_ll
                tankDescription = tankDescriptions[205];
            }
            else if (iconId == "ussr_m3_stuart_ll")
            {
                //0.9.15.1 replay tank name changed to r67_m3_ll
                tankDescription = tankDescriptions[206];
            }
            else if (iconId == "ussr_a_32")
            {
                //0.9.15.1 replay tank name changed to r68_a_32
                tankDescription = tankDescriptions[207];
            }
            else if (iconId == "ussr_su_85i")
            {
                //0.9.15.1 replay tank name changed to r78_su_85i
                tankDescription = tankDescriptions[210];
            }
            else if (iconId == "ussr_su76i")
            {
                //0.9.15.1 replay tank name changed to r50_su76i
                tankDescription = tankDescriptions[212];
            }
            else if (iconId == "ussr_tetrarch_ll")
            {
                //0.9.15.1 replay tank name changed to r84_tetrarch_ll
                tankDescription = tankDescriptions[213];
            }
            else if (iconId == "ussr_su100y")
            {
                //0.9.15.1 replay tank name changed to r49_su100y
                tankDescription = tankDescriptions[214];
            }
            else if (iconId == "ussr_su122_44")
            {
                //0.9.15.1 replay tank name changed to r89_su122_44
                tankDescription = tankDescriptions[216];
            }
            else if (iconId == "ussr_t44_122")
            {
                //0.9.15.1 replay tank name changed to r99_t44_122
                tankDescription = tankDescriptions[222];
            }
            else if (iconId == "ussr_t44_85")
            {
                //0.9.15.1 replay tank name changed to r98_t44_85
                tankDescription = tankDescriptions[223];
            }
            else if (iconId == "ussr_object_907a")
            {
                //0.9.15.1 replay tank name changed to r95_object_907a
                tankDescription = tankDescriptions[237];
            }
            else if (iconId == "ussr_is_7_fallout")
            {
                //0.9.15.1 replay tank name changed to r45_is_7_fallout
                tankDescription = tankDescriptions[251];
            }
            else if (iconId == "ussr_isu_152_igr")
            {
                //0.9.15.1 replay tank name changed to r47_isu_152_igr
                tankDescription = tankDescriptions[151];
            }
            else if (iconId == "ussr_t_44_igr")
            {
                //0.9.15.1 replay tank name changed to r20_t_44_igr
                tankDescription = tankDescriptions[152];
            }
            else if (iconId == "ussr_kv2_igr")
            {
                //0.9.15.1 replay tank name changed to r77_kv2_igr
                tankDescription = tankDescriptions[155];
            }
            else if (iconId == "ussr_valentine_ll_igr")
            {
                //0.9.15.1 replay tank name changed to r31_valentine_ll_igr
                tankDescription = tankDescriptions[158];
            }
            else if (iconId == "ussr_is_7_igr")
            {
                //0.9.15.1 replay tank name changed to r45_is_7_igr
                tankDescription = tankDescriptions[159];
            }
            else if (iconId == "ussr_t_26_bot")
            {
                //0.9.15.1 replay tank name changed to r09_t_26_bot
                tankDescription = tankDescriptions[161];
            }
            else if (iconId == "germany_g14_pziii_a")
            {
                //0.9.17 replay tank name changed to g102_pz_iii
                tankDescription = tankDescriptions[10019];
            }
            else if (iconId == "ussr_r38_kv_220")
            {
                //0.9.17 replay tank name changed to r38_kv_220_beta
                tankDescription = tankDescriptions[200];
            }
            else if (iconId == "ussr_r38_kv_220_action")
            {
                //0.9.17 replay tank name changed to r38_kv_220
                tankDescription = tankDescriptions[211];
            }
            return tankDescription;
        }
    }
}
