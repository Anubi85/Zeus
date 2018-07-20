using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Shapes;
using Zeus.UI.Enums;
using Zeus.UI.Events;
using Zeus.UI.Themes.Enums;

namespace Zeus.UI.Controls
{
    /// <summary>
    /// Zeus styled clock.
    /// </summary>
    [TemplatePart(Name = c_ElementHourIncreaseButton, Type = typeof(ZeusButton))]
    [TemplatePart(Name = c_ElementHourDecreaseButton, Type = typeof(ZeusButton))]
    [TemplatePart(Name = c_ElementHourDisplay, Type = typeof(ZeusTextBlock))]
    [TemplatePart(Name = c_ElementMinuteIncreaseButton, Type = typeof(ZeusButton))]
    [TemplatePart(Name = c_ElementMinuteDecreaseButton, Type = typeof(ZeusButton))]
    [TemplatePart(Name = c_ElementMinuteDisplay, Type = typeof(ZeusTextBlock))]
    [TemplatePart(Name = c_ElementSecondIncreaseButton, Type = typeof(ZeusButton))]
    [TemplatePart(Name = c_ElementSecondDecreaseButton, Type = typeof(ZeusButton))]
    [TemplatePart(Name = c_ElementSecondDisplay, Type = typeof(ZeusTextBlock))]
    [TemplatePart(Name = c_ElementHourView, Type = typeof(Grid))]
    [TemplatePart(Name = c_ElementMinSecView, Type = typeof(Grid))]
    [TemplatePart(Name = c_ElementClock, Type = typeof(Canvas))]
    public class ZeusClock : Control
    {
        #region Enum

        /// <summary>
        /// Enumarates the possible <see cref="SelectedTime"/> setters.
        /// </summary>
        private enum SelectedTimeSetters
        {
            Property,
            IncreaseDecreaseButton,
            SelectButton
        }

        #endregion

        #region Constants

        /// <summary>
        /// Name of the hour increase button element in the control template.
        /// </summary>
        private const string c_ElementHourIncreaseButton = "PART_HourIncreaseButton";
        /// <summary>
        /// Name of the hour decrease button element in the control template.
        /// </summary>
        private const string c_ElementHourDecreaseButton = "PART_HourDecreaseButton";
        /// <summary>
        /// Name of the minute increase button element in the control template.
        /// </summary>
        private const string c_ElementMinuteIncreaseButton = "PART_MinuteIncreaseButton";
        /// <summary>
        /// Name of the minute decrease button element in the control template.
        /// </summary>
        private const string c_ElementMinuteDecreaseButton = "PART_MinuteDecreaseButton";
        /// <summary>
        /// Name of the second increase button element in the control template.
        /// </summary>
        private const string c_ElementSecondIncreaseButton = "PART_SecondIncreaseButton";
        /// <summary>
        /// Name of the second decrease button element in the control template.
        /// </summary>
        private const string c_ElementSecondDecreaseButton = "PART_SecondDecreaseButton";
        /// <summary>
        /// Name of the hour display text box element in the control template.
        /// </summary>
        private const string c_ElementHourDisplay = "PART_HourDisplay";
        /// <summary>
        /// Name of the minute display text box element in the control template.
        /// </summary>
        private const string c_ElementMinuteDisplay = "PART_MinuteDisplay";
        /// <summary>
        /// Name of the second display text box element in the control template.
        /// </summary>
        private const string c_ElementSecondDisplay = "PART_SecondDisplay";
        /// <summary>
        /// Name of the hour view grid element in the control template.
        /// </summary>
        private const string c_ElementHourView = "PART_HourView";
        /// <summary>
        /// Name of the minutes/seconds view grid element in the control template.
        /// </summary>
        private const string c_ElementMinSecView = "PART_MinSecView";
        /// <summary>
        /// Name of the clock canvas element in the control template.
        /// </summary>
        private const string c_ElementClock = "PART_Clock";

        #endregion

        #region Fields

        /// <summary>
        /// The text block control that will display selected time hour.
        /// </summary>
        private ZeusTextBlock m_HourDisplay;
        /// <summary>
        /// The button that allows to increase the selected time hours.
        /// </summary>
        private ZeusButton m_IncreaseHourButton;
        /// <summary>
        /// The button that allows to decrease the selected time hours.
        /// </summary>
        private ZeusButton m_DecreaseHourButton;
        /// <summary>
        /// The text block control that will display selected time minutes.
        /// </summary>
        private ZeusTextBlock m_MinuteDisplay;
        /// <summary>
        /// The button that allows to increase the selected time minutes.
        /// </summary>
        private ZeusButton m_IncreaseMinuteButton;
        /// <summary>
        /// The button that allows to decrease the selected time minutes.
        /// </summary>
        private ZeusButton m_DecreaseMinuteButton;
        /// <summary>
        /// The text block control that will display selected time seconds.
        /// </summary>
        private ZeusTextBlock m_SecondDisplay;
        /// <summary>
        /// The button that allows to increase the selected time seconds.
        /// </summary>
        private ZeusButton m_IncreaseSecondButton;
        /// <summary>
        /// The button that allows to decrease the selected time seconds.
        /// </summary>
        private ZeusButton m_DecreaseSecondButton;
        /// <summary>
        /// The grid that will display the hour selection buttons.
        /// </summary>
        private Grid m_HourView;
        /// <summary>
        /// The grid that will display the minute/second selection buttons.
        /// </summary>
        private Grid m_MinSecView;
        /// <summary>
        /// The cancas that will display the clock graphic representation.
        /// </summary>
        private Canvas m_ClockCanvas;
        /// <summary>
        /// The line that draws the clock hours handle.
        /// </summary>
        private Line m_HoursHandle;
        /// <summary>
        /// The line that draws the clock minutes handle.
        /// </summary>
        private Line m_MinutesHandle;
        /// <summary>
        /// The line that draws the clock seconds handle (long part).
        /// </summary>
        private Line m_SecondsHandle1;
        /// <summary>
        /// The line that draws the clock seconds handle (short part).
        /// </summary>
        private Line m_SecondsHandle2;
        /// <summary>
        /// Length in pixel of the hours handle.
        /// </summary>
        private double m_HoursHandleLength;
        /// <summary>
        /// Length in pixel of the minutes handle.
        /// </summary>
        private double m_MinutesHandleLength;
        /// <summary>
        /// Length in pixel of the seconds handle (long part).
        /// </summary>
        private double m_SecondsHandleLength1;
        /// <summary>
        /// Length in pixel of the minutes handle (short part).
        /// </summary>
        private double m_SecondsHandleLength2;
        /// <summary>
        /// The setter of the <see cref="SelectedTime"/> property.
        /// </summary>
        private SelectedTimeSetters m_SelectedTimeSetter;

        #endregion

        #region Constructor

        /// <summary>
        /// Overrides metadata for proper style handling.
        /// </summary>
        static ZeusClock()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZeusClock), new FrameworkPropertyMetadata(typeof(ZeusClock)));
            ColorProperty = ZeusWindowBase.ColorProperty.AddOwner(typeof(ZeusClock), new FrameworkPropertyMetadata(ZeusColorStyles.Blue, FrameworkPropertyMetadataOptions.Inherits));
        }

        #endregion

        #region Dependency properties

        /// <summary>
        /// <see cref="DependencyProperty"/> that handle <see cref="ZeusClock"/> color styles.
        /// </summary>
        public static readonly DependencyProperty ColorProperty;
        /// <summary>
        /// <see cref="DependencyProperty"/> that handle <see cref="ZeusClock"/> selected time.
        /// </summary>
        public static readonly DependencyProperty SelectedTimeProperty = DependencyProperty.Register("SelectedTime", typeof(TimeSpan), typeof(ZeusClock), new PropertyMetadata(TimeSpan.Zero, OnSelectedTimeChanged));
        /// <summary>
        /// <see cref="DependencyProperty"/> that handle <see cref="ZeusClock"/> display mode.
        /// </summary>
        public static readonly DependencyProperty DisplayModeProperty = DependencyProperty.Register("DisplayMode", typeof(ZeusClockMode), typeof(ZeusClock), new PropertyMetadata(ZeusClockMode.Clock));

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets <see cref="ZeusClock"/> color style.
        /// </summary>
        public ZeusColorStyles Color
        {
            get { return (ZeusColorStyles)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }
        /// <summary>
        /// Gets or sets the selected time.
        /// </summary>
        public TimeSpan SelectedTime
        {
            get { return (TimeSpan)GetValue(SelectedTimeProperty); }
            set { SetValue(SelectedTimeProperty, value); }
        }
        /// <summary>
        /// Gets or sets the display mode.
        /// </summary>
        public ZeusClockMode DisplayMode
        {
            get { return (ZeusClockMode)GetValue(DisplayModeProperty); }
            set { SetValue(DisplayModeProperty, value); }
        }

        #endregion

        #region Routed events

        /// <summary>
        /// Occours when the selected time changes.
        /// </summary>
        public static readonly RoutedEvent SelectedTimeChangedEvent = EventManager.RegisterRoutedEvent("SelectedTimeChanged", RoutingStrategy.Direct, typeof(EventHandler<ZeusSelectedTimeChangedEventArgs>), typeof(ZeusClock));

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the <see cref="SelectedTime" /> property is changed.
        /// </summary>
        public event EventHandler<ZeusSelectedTimeChangedEventArgs> SelectedTimeChanged
        {
            add { AddHandler(SelectedTimeChangedEvent, value); }
            remove { RemoveHandler(SelectedTimeChangedEvent, value); }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Builds the visual tree for the <see cref="ZeusClock"/> control when a new template is applied.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            SetupControls(ref m_HourDisplay, ref m_IncreaseHourButton, ref m_DecreaseHourButton, c_ElementHourDisplay, c_ElementHourIncreaseButton, c_ElementHourDecreaseButton, "hh");
            SetupControls(ref m_MinuteDisplay, ref m_IncreaseMinuteButton, ref m_DecreaseMinuteButton, c_ElementMinuteDisplay, c_ElementMinuteIncreaseButton, c_ElementMinuteDecreaseButton, "mm");
            SetupControls(ref m_SecondDisplay, ref m_IncreaseSecondButton, ref m_DecreaseSecondButton, c_ElementSecondDisplay, c_ElementSecondIncreaseButton, c_ElementSecondDecreaseButton, "ss");
            if (m_HourView != null)
            {
                ClearSelectionView(m_HourView);
            }
            if (m_MinSecView != null)
            {
                ClearSelectionView(m_MinSecView);
            }
            if (m_ClockCanvas != null)
            {
                m_ClockCanvas.Children.Clear();
                m_ClockCanvas.Loaded -= InitializeClockCanvas;
            }
            m_HourView = GetTemplateChild(c_ElementHourView) as Grid;
            m_MinSecView = GetTemplateChild(c_ElementMinSecView) as Grid;
            m_ClockCanvas = GetTemplateChild(c_ElementClock) as Canvas;
            if (m_HourView != null)
            {
                PopulateSelectionView(m_HourView, 1);
            }
            if (m_MinSecView != null)
            {
                PopulateSelectionView(m_MinSecView, 5);
            }
            if (m_ClockCanvas != null)
            {
                m_ClockCanvas.Loaded += InitializeClockCanvas;
            }
        }
        /// <summary>
        /// Clean up a previously configured hour view.
        /// </summary>
        /// /// <param name="view">The control that has to be cleared.</param>
        private void ClearSelectionView(Grid view)
        {
            foreach (ZeusClockButton btn in view.Children.OfType<ZeusClockButton>())
            {
                btn.Click -= OnClockButtonClick;
            }
        }
        /// <summary>
        /// Populates the hour view with the hour selection buttons.
        /// </summary>
        /// <param name="view">The control that has to be populated.</param>
        /// <param name="factor">The multiplication factor used for content computation.</param>
        private void PopulateSelectionView(Grid view, int factor)
        {
            for (int i = 0; i < view.RowDefinitions.Count; i++)
            {
                for (int j = 0; j < view.ColumnDefinitions.Count; j++)
                {
                    ZeusClockButton btn = new ZeusClockButton();
                    btn.SetValue(Grid.RowProperty, i);
                    btn.SetValue(Grid.ColumnProperty, j);
                    btn.Content = (i * view.ColumnDefinitions.Count + j) * factor;
                    btn.Click += OnClockButtonClick;
                    view.Children.Add(btn);
                }
            }
        }
        /// <summary>
        /// Configures the display, increase and decrease controls for one of the selected time parts.
        /// </summary>
        /// <param name="display">The display control storage field.</param>
        /// <param name="increase">The increase control storage field.</param>
        /// <param name="decrease">The decrease control storage field.</param>
        /// <param name="displayName">The display control template name.</param>
        /// <param name="increaseName">The increase control template name.</param>
        /// <param name="decreaseName">The decrease control template name.</param>
        /// <param name="format">The format string for the displyed data.</param>
        private void SetupControls(ref ZeusTextBlock display, ref ZeusButton increase, ref ZeusButton decrease, string displayName, string increaseName, string decreaseName, string format)
        {
            //unregister event handlers
            if (display != null)
            {
                display.Click -= OnDisplayClick;
            }
            if (increase != null)
            {
                increase.Click -= OnIncreaseButtonClick;
            }
            if (decrease != null)
            {
                decrease.Click -= OnDecreaseButtonClick;
            }
            //get template childs
            display = GetTemplateChild(displayName) as ZeusTextBlock;
            increase = GetTemplateChild(increaseName) as ZeusButton;
            decrease = GetTemplateChild(decreaseName) as ZeusButton;
            //register event handlers and apply bindings
            if (display != null)
            {
                display.SetBinding(TextBlock.TextProperty, new Binding(SelectedTimeProperty.Name) { Source = this, StringFormat = format });
                display.Click += OnDisplayClick;
            }
            if (increase != null)
            {
                increase.Click += OnIncreaseButtonClick;
            }
            if (decrease != null)
            {
                decrease.Click += OnDecreaseButtonClick;
            }
        }
        /// <summary>
        /// Handle the click event on the hour display control.
        /// </summary>
        /// <param name="sender">The object that trigger the event.</param>
        /// <param name="e">Information about the occurred event.</param>
        private void OnDisplayClick(object sender, RoutedEventArgs e)
        {
            if (sender.Equals(m_HourDisplay))
            {
                DisplayMode = ZeusClockMode.Hours;
                SelectButton(m_HourView, SelectedTime.Hours);
                return;
            }
            if (sender.Equals(m_MinuteDisplay))
            {
                DisplayMode = ZeusClockMode.Minutes;
                SelectButton(m_MinSecView, SelectedTime.Minutes);
                return;
            }
            if (sender.Equals(m_SecondDisplay))
            {
                DisplayMode = ZeusClockMode.Seconds;
                SelectButton(m_MinSecView, SelectedTime.Seconds);
                return;
            }
        }
        /// <summary>
        /// Handle the click event on the increase button.
        /// </summary>
        /// <param name="sender">The object that trigger the event.</param>
        /// <param name="e">Information about the occurred event.</param>
        private void OnIncreaseButtonClick(object sender, RoutedEventArgs e)
        {
            if (sender.Equals(m_IncreaseHourButton))
            {
                m_SelectedTimeSetter = SelectedTimeSetters.IncreaseDecreaseButton;
                SelectedTime = new TimeSpan((SelectedTime.Hours + 1) % 24, SelectedTime.Minutes, SelectedTime.Seconds);
                return;
            }
            if (sender.Equals(m_IncreaseMinuteButton))
            {
                m_SelectedTimeSetter = SelectedTimeSetters.IncreaseDecreaseButton;
                SelectedTime = new TimeSpan(SelectedTime.Hours, (SelectedTime.Minutes + 1) % 60, SelectedTime.Seconds);
                return;
            }
            if (sender.Equals(m_IncreaseSecondButton))
            {
                m_SelectedTimeSetter = SelectedTimeSetters.IncreaseDecreaseButton;
                SelectedTime = new TimeSpan(SelectedTime.Hours, SelectedTime.Minutes, (SelectedTime.Seconds + 1) % 60);
                return;
            }
        }
        /// <summary>
        /// Handle the click event on the decrease button.
        /// </summary>
        /// <param name="sender">The object that trigger the event.</param>
        /// <param name="e">Information about the occurred event.</param>
        private void OnDecreaseButtonClick(object sender, RoutedEventArgs e)
        {
            if (sender.Equals(m_DecreaseHourButton))
            {
                m_SelectedTimeSetter = SelectedTimeSetters.IncreaseDecreaseButton;
                SelectedTime = new TimeSpan(SelectedTime.Hours == 0 ? 23 : SelectedTime.Hours - 1, SelectedTime.Minutes, SelectedTime.Seconds);
                return;
            }
            if (sender.Equals(m_DecreaseMinuteButton))
            {
                m_SelectedTimeSetter = SelectedTimeSetters.IncreaseDecreaseButton;
                SelectedTime = new TimeSpan(SelectedTime.Hours, SelectedTime.Minutes == 0 ? 59 : SelectedTime.Minutes - 1, SelectedTime.Seconds);
                return;
            }
            if (sender.Equals(m_DecreaseSecondButton))
            {
                m_SelectedTimeSetter = SelectedTimeSetters.IncreaseDecreaseButton;
                SelectedTime = new TimeSpan(SelectedTime.Hours, SelectedTime.Minutes, SelectedTime.Seconds == 0 ? 59 : SelectedTime.Seconds - 1);
                return;
            }
        }
        /// <summary>
        /// Handle the click event on the hour button.
        /// </summary>
        /// <param name="sender">The object that trigger the event.</param>
        /// <param name="e">Information about the occurred event.</param>
        private void OnClockButtonClick(object sender, RoutedEventArgs e)
        {
            ZeusClockButton btn = sender as ZeusClockButton;
            if (btn != null)
            {
                switch (DisplayMode)
                {
                    case ZeusClockMode.Hours:
                        m_SelectedTimeSetter = SelectedTimeSetters.SelectButton;
                        SelectedTime = new TimeSpan((int)btn.Content, SelectedTime.Minutes, SelectedTime.Seconds);
                        SelectButton(m_HourView, SelectedTime.Hours);
                        break;
                    case ZeusClockMode.Minutes:
                        m_SelectedTimeSetter = SelectedTimeSetters.SelectButton;
                        SelectedTime = new TimeSpan(SelectedTime.Hours, (int)btn.Content, SelectedTime.Seconds);
                        SelectButton(m_MinSecView, SelectedTime.Minutes);
                        break;
                    case ZeusClockMode.Seconds:
                        m_SelectedTimeSetter = SelectedTimeSetters.SelectButton;
                        SelectedTime = new TimeSpan(SelectedTime.Hours, SelectedTime.Minutes, (int)btn.Content);
                        SelectButton(m_MinSecView, SelectedTime.Seconds);
                        break;
                }
            }
            DisplayMode = ZeusClockMode.Clock;
        }
        /// <summary>
        /// Set the IsSelected flag of the <see cref="ZeusClockButton"/> in the given view.
        /// </summary>
        /// <param name="view">The view which buttons flag has to be set.</param>
        /// <param name="value">The current selected value for the given view.</param>
        private void SelectButton(Grid view, int value)
        {
            foreach (ZeusClockButton btn in view.Children.OfType<ZeusClockButton>())
            {
                btn.IsSelected = btn.Content.Equals(value);
            }
        }
        /// <summary>
        /// <see cref="SelectedTimeProperty"/> property changed handler.
        /// </summary>
        /// <param name="d"><see cref="ZeusClock"/> that changed its <see cref="SelectedTime"/>.</param>
        /// <param name="e"><see cref="DependencyPropertyChangedEventArgs"/>.</param>
        private static void OnSelectedTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ZeusClock zc = d as ZeusClock;
            zc.DrawClock();
            zc.RaiseEvent(new ZeusSelectedTimeChangedEventArgs(SelectedTimeChangedEvent, (TimeSpan?)e.OldValue, (TimeSpan?)e.NewValue, zc.m_SelectedTimeSetter == SelectedTimeSetters.IncreaseDecreaseButton));
            zc.m_SelectedTimeSetter = SelectedTimeSetters.Property;
        }
        /// <summary>
        /// Initializes the clock canvas by adding all the required elements for clock drawing.
        /// </summary>
        /// <param name="sender">The object that trigger the event.</param>
        /// <param name="e">Information about the occurred event.</param>
        private void InitializeClockCanvas(object sender, RoutedEventArgs e)
        {
            m_ClockCanvas.Children.Clear();
            //compute clock parameters
            double clockDiameter = Math.Min(m_ClockCanvas.ActualWidth, m_ClockCanvas.ActualHeight);
            double clockCenterX = m_ClockCanvas.ActualWidth / 2;
            double clockCenterY = m_ClockCanvas.ActualHeight / 2;
            m_HoursHandleLength = clockDiameter / 4;
            m_MinutesHandleLength = clockDiameter / 8 * 3;
            m_SecondsHandleLength1 = clockDiameter / 16 * 7;
            m_SecondsHandleLength2 = clockDiameter / 16 * 2;
            double tickStart = clockDiameter / 16 * 7;
            double tickEnd = clockDiameter / 2;
            //add clock border
            Ellipse clockBorder = new Ellipse();
            clockBorder.SnapsToDevicePixels = true;
            clockBorder.Stroke = Foreground;
            clockBorder.StrokeThickness = 1;
            clockBorder.Height = clockDiameter;
            clockBorder.Width = clockDiameter;
            Canvas.SetLeft(clockBorder, clockCenterX - clockDiameter / 2);
            Canvas.SetTop(clockBorder, clockCenterY - clockDiameter / 2);
            m_ClockCanvas.Children.Add(clockBorder);
            //add clock ticks
            for (int i = 0; i < 12; i++)
            {
                Line tick = new Line();
                tick.SnapsToDevicePixels = true;
                tick.Stroke = Foreground;
                tick.StrokeThickness = 1;
                double angle = Math.PI / 6 * i;
                tick.X1 = Math.Cos(angle) * tickStart;
                tick.Y1 = Math.Sin(angle) * tickStart;
                tick.X2 = Math.Cos(angle) * tickEnd;
                tick.Y2 = Math.Sin(angle) * tickEnd;
                Canvas.SetLeft(tick, clockCenterX);
                Canvas.SetTop(tick, clockCenterY);
                m_ClockCanvas.Children.Add(tick);
            }
            //add hours handle
            m_HoursHandle = new Line();
            m_HoursHandle.SnapsToDevicePixels = true;
            m_HoursHandle.Stroke = Foreground;
            m_HoursHandle.StrokeThickness = 2;
            m_HoursHandle.X1 = 0;
            m_HoursHandle.Y1 = 0;
            m_HoursHandle.X2 = 0;
            m_HoursHandle.Y2 = 0;
            Canvas.SetLeft(m_HoursHandle, clockCenterX);
            Canvas.SetTop(m_HoursHandle, clockCenterY);
            m_ClockCanvas.Children.Add(m_HoursHandle);
            //add minutes handle
            m_MinutesHandle = new Line();
            m_MinutesHandle.SnapsToDevicePixels = true;
            m_MinutesHandle.Stroke = Foreground;
            m_MinutesHandle.StrokeThickness = 2;
            m_MinutesHandle.X1 = 0;
            m_MinutesHandle.Y1 = 0;
            m_MinutesHandle.X2 = 0;
            m_MinutesHandle.Y2 = 0;
            Canvas.SetLeft(m_MinutesHandle, clockCenterX);
            Canvas.SetTop(m_MinutesHandle, clockCenterY);
            m_ClockCanvas.Children.Add(m_MinutesHandle);
            //add seconds handle
            m_SecondsHandle1 = new Line();
            m_SecondsHandle2 = new Line();
            m_SecondsHandle1.SnapsToDevicePixels = true;
            m_SecondsHandle2.SnapsToDevicePixels = true;
            m_SecondsHandle1.Stroke = BorderBrush;
            m_SecondsHandle2.Stroke = BorderBrush;
            m_SecondsHandle1.StrokeThickness = 1;
            m_SecondsHandle2.StrokeThickness = 1;
            m_SecondsHandle1.X1 = 0;
            m_SecondsHandle2.X1 = 0;
            m_SecondsHandle1.Y1 = 0;
            m_SecondsHandle2.Y1 = 0;
            m_SecondsHandle1.X2 = 0;
            m_SecondsHandle2.X2 = 0;
            m_SecondsHandle1.Y2 = 0;
            m_SecondsHandle2.Y2 = 0;
            Canvas.SetLeft(m_SecondsHandle1, clockCenterX);
            Canvas.SetLeft(m_SecondsHandle2, clockCenterX);
            Canvas.SetTop(m_SecondsHandle1, clockCenterY);
            Canvas.SetTop(m_SecondsHandle2, clockCenterY);
            m_ClockCanvas.Children.Add(m_SecondsHandle1);
            m_ClockCanvas.Children.Add(m_SecondsHandle2);
            //add dot
            Ellipse dot = new Ellipse();
            dot.SnapsToDevicePixels = true;
            dot.Fill = BorderBrush;
            dot.Height = clockDiameter / 16;
            dot.Width = clockDiameter / 16;
            Canvas.SetLeft(dot, clockCenterX - clockDiameter / 32);
            Canvas.SetTop(dot, clockCenterY - clockDiameter / 32);
            m_ClockCanvas.Children.Add(dot);
            DrawClock();
        }
        /// <summary>
        /// Draws the clock graphic representation.
        /// </summary>
        private void DrawClock()
        {
            if (m_ClockCanvas != null)
            {
                double hourTheta = (SelectedTime.TotalHours % 12 - 3) / 6 * Math.PI;
                m_HoursHandle.X2 = Math.Cos(hourTheta) * m_HoursHandleLength;
                m_HoursHandle.Y2 = Math.Sin(hourTheta) * m_HoursHandleLength;
                double minuteTheta = (SelectedTime.TotalMinutes % 60 - 15) / 30 * Math.PI;
                m_MinutesHandle.X2 = Math.Cos(minuteTheta) * m_MinutesHandleLength;
                m_MinutesHandle.Y2 = Math.Sin(minuteTheta) * m_MinutesHandleLength;
                double secondTheta = (SelectedTime.TotalSeconds % 60 - 15) / 30 * Math.PI;
                m_SecondsHandle1.X2 = Math.Cos(secondTheta) * m_SecondsHandleLength1;
                m_SecondsHandle2.X2 = -Math.Cos(secondTheta) * m_SecondsHandleLength2;
                m_SecondsHandle1.Y2 = Math.Sin(secondTheta) * m_SecondsHandleLength1;
                m_SecondsHandle2.Y2 = -Math.Sin(secondTheta) * m_SecondsHandleLength2;
            }
        }

        #endregion
    }
}
