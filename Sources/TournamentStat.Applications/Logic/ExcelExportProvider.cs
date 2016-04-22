using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using TournamentStat.Applications.ViewModel;
using WotDossier.Applications.Logic;
using WotDossier.Applications.ViewModel.Rows;
using WotDossier.Framework;

namespace TournamentStat.Applications.Logic
{
    public class ExcelExportProvider
    {
        public static class EffRangeColors
        {
            public static Color Black = Color.FromArgb(0, 0, 0);
            public static Color BlackRed = Color.FromArgb(94, 0, 0);
            public static Color Red = Color.FromArgb(254, 14, 0);
            public static Color Purple = Color.FromArgb(208, 66, 243);
            public static Color Blue = Color.FromArgb(2, 201, 179);
            public static Color Green = Color.FromArgb(96, 255, 0);
            public static Color Yellow = Color.FromArgb(248, 244, 3);
            public static Color Orange = Color.FromArgb(254, 121, 3);
        }

        private string _format = "# ##0.00";

        public void Export(string fileName, TournamentStatSettings settings, List<ITankStatisticRow> statisticRows)
        {
            using (new WaitCursor())
            {
                using (ExcelPackage package = new ExcelPackage(new FileInfo(fileName)))
                {
                    AddParticipantsSheet(package, settings, statisticRows);
                    AddStatSheet(package, settings, statisticRows);
                    AddNominationsSheets(package, settings, statisticRows);

                    package.Save();
                }
            }
        }

        private void AddNominationsSheets(ExcelPackage package, TournamentStatSettings settings, List<ITankStatisticRow> statisticRows)
        {
            foreach (var nomination in settings.TournamentNominations)
            {
                AddNominationSheets(package, nomination, statisticRows);
            }
        }

        private void AddNominationSheets(ExcelPackage package, TournamentNomination nomination, List<ITankStatisticRow> statisticRows)
        {
            var excelWorksheet = package.Workbook.Worksheets.Add(Properties.Resources.Out_Sheet_Nomination_Prefix + nomination.Nomination);
            int row = 2;

            int column = 1;

            excelWorksheet.Cells[1, column++].Value = "место";
            excelWorksheet.Cells[1, column++].Value = "игрок";
            excelWorksheet.Cells[1, column++].Value = "танк";
            if (nomination.Criterion == TournamentCriterion.Damage
                || nomination.Criterion == TournamentCriterion.DamageWithArmor
                || nomination.Criterion == TournamentCriterion.DamageWithAssist)
            {
                excelWorksheet.Cells[1, column++].Value = "урон";
                excelWorksheet.Cells[1, column++].Value = "ПП";
            }
            if (nomination.Criterion == TournamentCriterion.DamageWithArmor)
            {
                excelWorksheet.Cells[1, column++].Value = "ППУ";
                excelWorksheet.Cells[1, column++].Value = "урон+ППУ";
            }
            if (nomination.Criterion == TournamentCriterion.DamageWithAssist)
            {
                excelWorksheet.Cells[1, column++].Value = "засвет";
                excelWorksheet.Cells[1, column++].Value = "урон+засвет";
            }
            if (nomination.Criterion == TournamentCriterion.WinPercent)
            {
                excelWorksheet.Cells[1, column++].Value = "ПП";
            }
            if (nomination.Criterion == TournamentCriterion.Frags)
            {
                excelWorksheet.Cells[1, column++].Value = "фраги";
            }

            excelWorksheet.Cells[1, 1, 1, column].Style.Fill.PatternType = ExcelFillStyle.Solid;
            excelWorksheet.Cells[1, 1, 1, column].Style.Fill.BackgroundColor.SetColor(Color.Gray);

            excelWorksheet.View.FreezePanes(2, 3);

            int index = 1;

            foreach (TankStatisticRowViewModelBase tankStatistic in NominationHelper.GetNominationResults(nomination, statisticRows))
            {
                column = 1;

                excelWorksheet.Cells[row, column++].Value = index++;
                excelWorksheet.Cells[row, column++].Value = tankStatistic.PlayerName;
                excelWorksheet.Cells[row, column++].Value = tankStatistic.Tank;
                if (nomination.Criterion == TournamentCriterion.Damage
                    || nomination.Criterion == TournamentCriterion.DamageWithArmor
                    || nomination.Criterion == TournamentCriterion.DamageWithAssist)
                {
                    if (nomination.Criterion == TournamentCriterion.Damage)
                    {
                        excelWorksheet.Cells[row, column].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        excelWorksheet.Cells[row, column].Style.Fill.BackgroundColor.SetColor(Color.Green);
                    }

                    excelWorksheet.Cells[row, column].Style.Numberformat.Format = _format;
                    excelWorksheet.Cells[row, column++].Value = tankStatistic.AvgDamageDealtForPeriod;
                }
                if (nomination.Criterion == TournamentCriterion.DamageWithArmor)
                {
                    excelWorksheet.Cells[row, column].Style.Numberformat.Format = _format;
                    excelWorksheet.Cells[row, column++].Value = tankStatistic.AvgPotentialDamageReceivedForPeriod;

                    excelWorksheet.Cells[row, column].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    excelWorksheet.Cells[row, column].Style.Fill.BackgroundColor.SetColor(Color.Green);
                    excelWorksheet.Cells[row, column].Style.Numberformat.Format = _format;
                    excelWorksheet.Cells[row, column++].Value = tankStatistic.AvgDamageDealtForPeriod +
                                                                tankStatistic.AvgPotentialDamageReceivedForPeriod;
                }
                if (nomination.Criterion == TournamentCriterion.DamageWithAssist)
                {
                    excelWorksheet.Cells[row, column].Style.Numberformat.Format = _format;
                    excelWorksheet.Cells[row, column++].Value = tankStatistic.AvgDamageAssistedForPeriod;

                    excelWorksheet.Cells[row, column].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    excelWorksheet.Cells[row, column].Style.Fill.BackgroundColor.SetColor(Color.Green);
                    excelWorksheet.Cells[row, column].Style.Numberformat.Format = _format;
                    excelWorksheet.Cells[row, column++].Value = tankStatistic.AvgDamageDealtForPeriod +
                                                                tankStatistic.AvgDamageAssistedForPeriod;
                }
                if (nomination.Criterion == TournamentCriterion.WinPercent || nomination.Criterion == TournamentCriterion.Damage
                    || nomination.Criterion == TournamentCriterion.DamageWithArmor
                    || nomination.Criterion == TournamentCriterion.DamageWithAssist)
                {
                    excelWorksheet.Cells[row, column].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    excelWorksheet.Cells[row, column].Style.Fill.BackgroundColor.SetColor(Color.Green);
                    excelWorksheet.Cells[row, column].Style.Numberformat.Format = "0.00 %";
                    excelWorksheet.Cells[row, column].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    excelWorksheet.Cells[row, column].Style.Fill.BackgroundColor.SetColor(GetWRColor(tankStatistic.WinsPercentForPeriod));
                    excelWorksheet.Cells[row, column++].Value = tankStatistic.WinsPercentForPeriod / 100;
                }
                if (nomination.Criterion == TournamentCriterion.Frags)
                {
                    excelWorksheet.Cells[row, column].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    excelWorksheet.Cells[row, column].Style.Fill.BackgroundColor.SetColor(Color.Green);
                    excelWorksheet.Cells[row, column].Style.Numberformat.Format = _format;
                    excelWorksheet.Cells[row, column++].Value = tankStatistic.AvgFragsForPeriod;
                }

                row++;
            }
            excelWorksheet.Cells[excelWorksheet.Dimension.Address].AutoFitColumns();
        }

        private void AddStatSheet(ExcelPackage package, TournamentStatSettings settings, List<ITankStatisticRow> statisticRows)
        {
            var excelWorksheet = package.Workbook.Worksheets.Add(Properties.Resources.Out_Sheet_Stat);
            int row = 3;

            int column = 1;

            excelWorksheet.Cells[2, column++].Value = "№";
            excelWorksheet.Cells[2, column++].Value = "игрок";
            excelWorksheet.Cells[2, column++].Value = "танк";
            //excelWorksheet.Cells[2, column++].Value = "режим";
            //excelWorksheet.Cells[2, column++].Value = "голд";
            excelWorksheet.Cells[2, column++].Value = "дата";

            int groupStartColumn = column;

            excelWorksheet.Cells[2, column++].Value = "бои";
            excelWorksheet.Cells[2, column++].Value = "ПП";
            excelWorksheet.Cells[2, column++].Value = "опыт";
            excelWorksheet.Cells[2, column++].Value = "фраги";
            excelWorksheet.Cells[2, column++].Value = "урон";
            excelWorksheet.Cells[2, column++].Value = "УпЗ";
            excelWorksheet.Cells[2, column++].Value = "ППУ";
            excelWorksheet.Cells[2, column++].Value = "WN8";

            excelWorksheet.Cells[1, groupStartColumn, 1, column - 1].Merge = true;
            excelWorksheet.Cells[1, groupStartColumn, 1, column - 1].Value = "серия";
            excelWorksheet.Cells[1, groupStartColumn, 1, column - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            excelWorksheet.Cells[1, groupStartColumn, 1, column - 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
            excelWorksheet.Cells[1, groupStartColumn, 1, column - 1].Style.Fill.BackgroundColor.SetColor(Color.Green);

            //excelWorksheet.Cells[2, column++].Value = "УпЗ";
            //excelWorksheet.Cells[2, column++].Value = "ППУ";
            //excelWorksheet.Cells[2, column++].Value = "захв";
            //excelWorksheet.Cells[2, column++].Value = "защ";
            //excelWorksheet.Cells[2, column++].Value = "свет";
            excelWorksheet.Cells[2, column++].Value = "реплеи на ресурсе игрока";
            excelWorksheet.Cells[2, column++].Value = "реплеи на ресурсе организатора";
            excelWorksheet.Cells[2, column++].Value = "патч";

            groupStartColumn = column;

            excelWorksheet.Cells[2, column++].Value = "бои";
            excelWorksheet.Cells[2, column++].Value = "поб";
            excelWorksheet.Cells[2, column++].Value = "ПП";
            excelWorksheet.Cells[2, column++].Value = "урон";
            //excelWorksheet.Cells[2, column++].Value = "опыт";
            //excelWorksheet.Cells[2, column++].Value = "фраги";

            excelWorksheet.Cells[1, groupStartColumn, 1, column - 1].Merge = true;
            excelWorksheet.Cells[1, groupStartColumn, 1, column - 1].Value = "статистика на начало турнира";
            excelWorksheet.Cells[1, groupStartColumn, 1, column - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            excelWorksheet.Cells[1, groupStartColumn, 1, column - 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
            excelWorksheet.Cells[1, groupStartColumn, 1, column - 1].Style.Fill.BackgroundColor.SetColor(Color.Yellow);

            groupStartColumn = column;

            excelWorksheet.Cells[2, column++].Value = "бои";
            excelWorksheet.Cells[2, column++].Value = "поб";
            excelWorksheet.Cells[2, column++].Value = "ПП";
            excelWorksheet.Cells[2, column++].Value = "урон";
            //excelWorksheet.Cells[2, column++].Value = "опыт";
            //excelWorksheet.Cells[2, column++].Value = "фраги";

            //excelWorksheet.Cells[2, column++].Value = "УпЗ";
            //excelWorksheet.Cells[2, column++].Value = "ППУ";
            //excelWorksheet.Cells[2, column++].Value = "захв";
            //excelWorksheet.Cells[2, column++].Value = "защ";
            //excelWorksheet.Cells[2, column++].Value = "свет";
            //excelWorksheet.Cells[2, column++].Value = "dosssier";

            excelWorksheet.Cells[1, groupStartColumn, 1, column - 1].Merge = true;
            excelWorksheet.Cells[1, groupStartColumn, 1, column - 1].Value = "статистика на конец турнира";
            excelWorksheet.Cells[1, groupStartColumn, 1, column - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            excelWorksheet.Cells[1, groupStartColumn, 1, column - 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
            excelWorksheet.Cells[1, groupStartColumn, 1, column - 1].Style.Fill.BackgroundColor.SetColor(Color.SteelBlue);

            excelWorksheet.Cells[2, 1, 2, column].Style.Fill.PatternType = ExcelFillStyle.Solid;
            excelWorksheet.Cells[2, 1, 2, column].Style.Fill.BackgroundColor.SetColor(Color.Gray);

            excelWorksheet.View.FreezePanes(3, 3);

            int index = 1;

            foreach (TankStatisticRowViewModelBase tankStatistic in statisticRows)
            {
                column = 1;

                excelWorksheet.Cells[row, column++].Value = index++;
                excelWorksheet.Cells[row, column++].Value = tankStatistic.PlayerName;
                excelWorksheet.Cells[row, column++].Value = tankStatistic.Tank;
                excelWorksheet.Cells[row, column].Value = tankStatistic.Updated;
                excelWorksheet.Cells[row, column++].Style.Numberformat.Format = "[$-419]d mmm;@";
                excelWorksheet.Cells[row, column++].Value = tankStatistic.BattlesCountDelta;
                excelWorksheet.Cells[row, column].Style.Numberformat.Format = "0.00 %";
                excelWorksheet.Cells[row, column].Style.Fill.PatternType = ExcelFillStyle.Solid;
                excelWorksheet.Cells[row, column].Style.Fill.BackgroundColor.SetColor(GetWRColor(tankStatistic.WinsPercentForPeriod));
                excelWorksheet.Cells[row, column++].Value = tankStatistic.WinsPercentForPeriod / 100;
                excelWorksheet.Cells[row, column].Style.Numberformat.Format = _format;
                excelWorksheet.Cells[row, column++].Value = tankStatistic.AvgXpForPeriod;
                excelWorksheet.Cells[row, column].Style.Numberformat.Format = _format;
                excelWorksheet.Cells[row, column++].Value = tankStatistic.AvgFragsForPeriod;
                excelWorksheet.Cells[row, column].Style.Numberformat.Format = _format;
                excelWorksheet.Cells[row, column++].Value = tankStatistic.AvgDamageDealtForPeriod;
                excelWorksheet.Cells[row, column].Style.Numberformat.Format = _format;
                excelWorksheet.Cells[row, column++].Value = tankStatistic.AvgDamageAssistedForPeriod;
                excelWorksheet.Cells[row, column].Style.Numberformat.Format = _format;
                excelWorksheet.Cells[row, column++].Value = tankStatistic.AvgPotentialDamageReceivedForPeriod;
                excelWorksheet.Cells[row, column].Style.Numberformat.Format = _format;
                excelWorksheet.Cells[row, column].Style.Fill.PatternType = ExcelFillStyle.Solid;
                excelWorksheet.Cells[row, column].Style.Fill.BackgroundColor.SetColor(GetWN8Color(tankStatistic.WN8RatingForPeriod));
                excelWorksheet.Cells[row, column++].Value = tankStatistic.WN8RatingForPeriod;

                excelWorksheet.Cells[row, column++].Value = tankStatistic.ReplaysUrlOwner;//"реплеи на ресурсе игрока";
                excelWorksheet.Cells[row, column++].Value = tankStatistic.ReplaysUrl;//"реплеи на ресурсе организатора";
                excelWorksheet.Cells[row, column++].Value = null;//"патч";

                excelWorksheet.Cells[row, column++].Value = tankStatistic.PreviousStatistic.BattlesCount;
                excelWorksheet.Cells[row, column++].Value = tankStatistic.PreviousStatistic.Wins;
                excelWorksheet.Cells[row, column].Style.Numberformat.Format = "0.00 %";
                excelWorksheet.Cells[row, column].Style.Fill.PatternType = ExcelFillStyle.Solid;
                excelWorksheet.Cells[row, column].Style.Fill.BackgroundColor.SetColor(GetWRColor(tankStatistic.PreviousStatistic.WinsPercent));
                excelWorksheet.Cells[row, column++].Value = tankStatistic.PreviousStatistic.WinsPercent / 100;
                excelWorksheet.Cells[row, column++].Value = tankStatistic.PreviousStatistic.DamageDealt;

                excelWorksheet.Cells[row, column++].Value = tankStatistic.BattlesCount;
                excelWorksheet.Cells[row, column++].Value = tankStatistic.Wins;
                excelWorksheet.Cells[row, column].Style.Numberformat.Format = "0.00 %";
                excelWorksheet.Cells[row, column].Style.Fill.PatternType = ExcelFillStyle.Solid;
                excelWorksheet.Cells[row, column].Style.Fill.BackgroundColor.SetColor(GetWRColor(tankStatistic.WinsPercent));
                excelWorksheet.Cells[row, column++].Value = tankStatistic.WinsPercent / 100;
                excelWorksheet.Cells[row, column++].Value = tankStatistic.DamageDealt;

                row++;
            }

            excelWorksheet.Cells[excelWorksheet.Dimension.Address].AutoFitColumns();
        }

        public Color GetWRColor(double? value)
        {
            if (value != null)
            {
                if (value >= Constants.Rating.WR_P5)
                    return EffRangeColors.Purple;
                if (value >= Constants.Rating.WR_P4)
                    return EffRangeColors.Blue;
                if (value >= Constants.Rating.WR_P3)
                    return EffRangeColors.Green;
                if (value >= Constants.Rating.WR_P2)
                    return EffRangeColors.Yellow;
                if (value >= Constants.Rating.WR_P1)
                    return EffRangeColors.Orange;
            }
            return EffRangeColors.Red;
        }

        public Color GetWN8Color(double? value)
        {
            if (value != null)
            {
                if (value >= Constants.Rating.WN8_P5)
                    return EffRangeColors.Purple;
                if (value >= Constants.Rating.WN8_P4)
                    return EffRangeColors.Blue;
                if (value >= Constants.Rating.WN8_P3)
                    return EffRangeColors.Green;
                if (value >= Constants.Rating.WN8_P2)
                    return EffRangeColors.Yellow;
                if (value >= Constants.Rating.WN8_P1)
                    return EffRangeColors.Orange;
            }
            return EffRangeColors.Red;
        }

        private void AddParticipantsSheet(ExcelPackage package, TournamentStatSettings settings, List<ITankStatisticRow> statisticRows)
        {
            var excelWorksheet = package.Workbook.Worksheets.Add(Properties.Resources.Out_Sheet_Participants);
            int row = 2;
            int column = 1;

            excelWorksheet.Cells[1, column++].Value = "номер";
            excelWorksheet.Cells[1, column++].Value = "игрок";
            excelWorksheet.Cells[1, column++].Value = "ссылка на профиль";
            excelWorksheet.Cells[1, column++].Value = "твитч";
            excelWorksheet.Cells[1, column++].Value = "моды";
            excelWorksheet.Cells[1, column++].Value = "дата";
            excelWorksheet.Cells[1, column++].Value = "танки";
            excelWorksheet.Cells[1, column++].Value = "призы";
            excelWorksheet.Cells[1, column++].Value = "серий";

            excelWorksheet.Cells[1, 1, 1, column].Style.Fill.PatternType = ExcelFillStyle.Solid;
            excelWorksheet.Cells[1, 1, 1, column].Style.Fill.BackgroundColor.SetColor(Color.Gray);

            excelWorksheet.View.FreezePanes(2, 3);

            int index = 1;

            foreach (var participant in settings.Players)
            {
                column = 1;

                excelWorksheet.Cells[row, column++].Value = index++;
                excelWorksheet.Cells[row, column++].Value = participant.PlayerName;

                excelWorksheet.Cells[row, column].Style.Font.Color.SetColor(Color.Blue);
                var link = $"http://worldoftanks.ru/community/accounts/{participant.AccountId}-{participant.PlayerName}/";
                excelWorksheet.Cells[row, column].Hyperlink = new ExcelHyperLink(link);
                excelWorksheet.Cells[row, column++].Value = link;

                excelWorksheet.Cells[row, column].Style.Font.Color.SetColor(Color.Blue);
                if (!string.IsNullOrEmpty(participant.TwitchUrl))
                {
                    excelWorksheet.Cells[row, column].Hyperlink = new ExcelHyperLink(participant.TwitchUrl);
                }
                excelWorksheet.Cells[row, column++].Value = participant.TwitchUrl;

                excelWorksheet.Cells[row, column++].Value = participant.Mods;
                excelWorksheet.Cells[row, column].Value = participant.StartDate;
                excelWorksheet.Cells[row, column++].Style.Numberformat.Format = "[$-419]d mmm;@";
                excelWorksheet.Cells[row, column++].Value = string.Join(", ",
                    statisticRows.Where(x => x.PlayerName == participant.PlayerName)
                        .GroupBy(x => x.Tank)
                        .Select(x => x.Key));
                excelWorksheet.Cells[row, column++].Value = null;
                excelWorksheet.Cells[row, column++].Value =
                    statisticRows.Count(x => x.PlayerName == participant.PlayerName);

                row++;
            }

            row = row + 2;
            excelWorksheet.Cells[row, 1, row, 7].Merge = true;
            excelWorksheet.Cells[row, 1, row, 7].Value =
                "желтый цвет - игрок заявился, статистика на начало не снята, серию играть нельзя";
            excelWorksheet.Cells[row, 1, row, 7].Style.Fill.PatternType = ExcelFillStyle.Solid;
            excelWorksheet.Cells[row, 1, row, 7].Style.Fill.BackgroundColor.SetColor(Color.Yellow);

            row++;
            excelWorksheet.Cells[row, 1, row, 7].Merge = true;
            excelWorksheet.Cells[row, 1, row, 7].Value =
                "зеленый цвет-игрок начал серию, статистика на начало зафиксирована";
            excelWorksheet.Cells[row, 1, row, 7].Style.Fill.PatternType = ExcelFillStyle.Solid;
            excelWorksheet.Cells[row, 1, row, 7].Style.Fill.BackgroundColor.SetColor(Color.Green);

            row++;
            excelWorksheet.Cells[row, 1, row, 7].Merge = true;
            excelWorksheet.Cells[row, 1, row, 7].Value =
                "синий цвет-игрок закончил 1 серию на танке";
            excelWorksheet.Cells[row, 1, row, 7].Style.Fill.PatternType = ExcelFillStyle.Solid;
            excelWorksheet.Cells[row, 1, row, 7].Style.Fill.BackgroundColor.SetColor(Color.SteelBlue);

            row++;
            excelWorksheet.Cells[row, 1, row, 7].Merge = true;
            excelWorksheet.Cells[row, 1, row, 7].Value =
                "фиолетовый цвет-игрок закончил 2+ серии на танке";
            excelWorksheet.Cells[row, 1, row, 7].Style.Fill.PatternType = ExcelFillStyle.Solid;
            excelWorksheet.Cells[row, 1, row, 7].Style.Fill.BackgroundColor.SetColor(Color.DarkViolet);

            //for (int i = 1; i < 9; i++)
            //{
            //    excelWorksheet.Column(1).AutoFit();
            //}

            excelWorksheet.Cells[excelWorksheet.Dimension.Address].AutoFitColumns();
        }
    }
}
