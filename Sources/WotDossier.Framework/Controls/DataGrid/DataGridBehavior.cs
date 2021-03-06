﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using WotDossier.Framework.Controls.DataGrid;

namespace WotDossier.Framework.Controls
{
    public class DataGridBehavior
    {
        #region DisplayRowNumber

        public static DependencyProperty DisplayRowNumberProperty =
            DependencyProperty.RegisterAttached("DisplayRowNumber",
                                                typeof(bool),
                                                typeof(DataGridBehavior),
                                                new FrameworkPropertyMetadata(false, OnDisplayRowNumberChanged));
        public static bool GetDisplayRowNumber(DependencyObject target)
        {
            return (bool)target.GetValue(DisplayRowNumberProperty);
        }
        public static void SetDisplayRowNumber(DependencyObject target, bool value)
        {
            target.SetValue(DisplayRowNumberProperty, value);
        }

        public static DependencyProperty InvertDefaultSortDirectionProperty =
            DependencyProperty.RegisterAttached("InvertDefaultSortDirection",
                                                typeof(bool),
                                                typeof(DataGridBehavior),
                                                new FrameworkPropertyMetadata(false, OnInvertDefaultSortDirectionChanged));

        private static void OnInvertDefaultSortDirectionChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            System.Windows.Controls.DataGrid dataGrid = target as System.Windows.Controls.DataGrid;
            dataGrid.Sorting += (s, se) => se.Column.SortDirection = se.Column.SortDirection ?? ListSortDirection.Ascending;
        }

        public static bool GetInvertDefaultSortDirection(DependencyObject target)
        {
            return (bool)target.GetValue(InvertDefaultSortDirectionProperty);
        }
        public static void SetInvertDefaultSortDirection(DependencyObject target, bool value)
        {
            target.SetValue(InvertDefaultSortDirectionProperty, value);
        }

        private static void OnDisplayRowNumberChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            System.Windows.Controls.DataGrid dataGrid = target as System.Windows.Controls.DataGrid;

            if ((bool)e.NewValue)
            {
                EventHandler<DataGridRowEventArgs> loadedRowHandler = null;
                loadedRowHandler = (sender, ea) =>
                {
                    if (GetDisplayRowNumber(dataGrid) == false)
                    {
                        dataGrid.LoadingRow -= loadedRowHandler;
                        return;
                    }
                    ea.Row.Header = GetRowHeader(ea.Row.GetIndex(), false);
                };
                dataGrid.LoadingRow += loadedRowHandler;

                ItemsChangedEventHandler itemsChangedHandler = null;
                itemsChangedHandler = (sender, ea) =>
                {
                    if (GetDisplayRowNumber(dataGrid) == false)
                    {
                        dataGrid.ItemContainerGenerator.ItemsChanged -= itemsChangedHandler;
                        return;
                    }
                    GetVisualChildCollection<DataGridRow>(dataGrid).
                        ForEach(d => d.Header = GetRowHeader(d.GetIndex(), dataGrid is FooterDataGrid));
                };
                dataGrid.ItemContainerGenerator.ItemsChanged += itemsChangedHandler;
            }
        }

        private static string GetRowHeader(int index, bool hasTotalRow)
        {
            if (hasTotalRow)
            {
                if (index == 0)
                {
                    return string.Empty;
                }
                return index.ToString();
            }
            return (index + 1).ToString();
        }

        #endregion // DisplayRowNumber

        #region Get Visuals

        private static List<T> GetVisualChildCollection<T>(object parent) where T : Visual
        {
            List<T> visualCollection = new List<T>();
            GetVisualChildCollection(parent as DependencyObject, visualCollection);
            return visualCollection;
        }

        private static void GetVisualChildCollection<T>(DependencyObject parent, List<T> visualCollection) where T : Visual
        {
            int count = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < count; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (child is T)
                {
                    visualCollection.Add(child as T);
                }
                if (child != null)
                {
                    GetVisualChildCollection(child, visualCollection);
                }
            }
        }

        #endregion // Get Visuals
    }
}
