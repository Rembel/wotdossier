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
            return tankDescription;
        }
    }
}
