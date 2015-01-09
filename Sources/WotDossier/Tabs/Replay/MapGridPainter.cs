using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace WotDossier.Tabs.Replay
{
    public class MapGridPainter : FrameworkElement
    {
        public static readonly DependencyProperty BoardSizeProperty = DependencyProperty.Register("BoardSize",
            typeof (int), typeof (MapGridPainter), new FrameworkPropertyMetadata(10, OnBoardSizeChanged),
            BoardSizeValidateCallback);

        private readonly Pen _blackPen;
        private readonly Brush _boardBrush;

        private readonly List<Visual> _visuals = new List<Visual>();

        private double _boardHeightFactor = 50;
        private int _boardSize;
        private DrawingVisual _boardVisual;
        private double _boardWidthFactor = 50;
        private Rect _goBoardHitBox;
        private Rect _goBoardRect;

        public MapGridPainter()
        {
            _boardBrush = new SolidColorBrush(Colors.Transparent);
            _blackPen = new Pen(Brushes.Black, 0.2);

            InitializeBoard(BoardSize);
        }

        #region Draw Methods

        private void DrawBoard()
        {
            _boardVisual = new DrawingVisual();

            using (DrawingContext dc = _boardVisual.RenderOpen())
            {
                dc.DrawRectangle(_boardBrush, new Pen(Brushes.Black, 0.2),
                    new Rect(0, 0, _boardSize*_boardWidthFactor,
                        _boardSize*_boardHeightFactor));
                
                for (int x = 0; x < _boardSize; x++)
                {
                    for (int y = 0; y < _boardSize; y++)
                    {
                        double posX = getPosX(x);
                        double posY = getPosY(y);
                        dc.DrawRectangle(_boardBrush, _blackPen,
                            new Rect(posX, posY, _boardWidthFactor, _boardHeightFactor));
                    }
                }
            }
        }
       
        #endregion

        protected override int VisualChildrenCount
        {
            get { return _visuals.Count; }
        }

        public int BoardSize
        {
            get { return (int) GetValue(BoardSizeProperty); }
            set { SetValue(BoardSizeProperty, value); }
        }

        private void InitializeBoard(int boardSize)
        {
            _visuals.ForEach(RemoveVisualChild);

            _visuals.Clear();

            _boardSize = boardSize;

            _goBoardRect = new Rect(new Size(_boardSize*_boardWidthFactor, _boardSize*_boardHeightFactor));
            _goBoardHitBox = _goBoardRect;
            _goBoardHitBox.Inflate((_boardWidthFactor/2), (_boardHeightFactor/2));

            Width = _goBoardRect.Width;
            Height = _goBoardRect.Height;

            DrawBoard();

            _visuals.Add(_boardVisual);

            _visuals.ForEach(AddVisualChild);
        }

        protected override Visual GetVisualChild(int index)
        {
            if (index < 0 || index >= _visuals.Count)
            {
                throw new ArgumentOutOfRangeException("index");
            }

            return _visuals[index];
        }

        private double getPosX(double value)
        {
            return _boardWidthFactor*value;
        }

        private double getPosY(double value)
        {
            return _boardHeightFactor*value;
        }

        private static void OnBoardSizeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            ((MapGridPainter) sender).InitializeBoard(((MapGridPainter) sender).BoardSize);
        }

        private static bool BoardSizeValidateCallback(object target)
        {
            if ((int) target < 2 || (int) target > 19)
                return false;

            return true;
        }
    }
}