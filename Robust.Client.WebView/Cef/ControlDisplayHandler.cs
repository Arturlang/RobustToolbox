using System;
using Robust.Client.UserInterface;
using Xilium.CefGlue;

namespace Robust.Client.WebView.Cef;

/// <summary>
///     Applies the cursor the page asks for to the hosting control.
///
///     Without this the pointer keeps whatever shape the control has regardless of
///     what is under it, so hovering a text input or a link inside a browser control
///     gives no I-beam or hand - the page looks inert even though it is live.
///
///     The shape is set on the control rather than pushed straight to the window,
///     because <see cref="UserInterfaceManager"/> reapplies the hovered control's
///     shape every frame and would immediately overwrite a direct call.
/// </summary>
internal sealed class ControlDisplayHandler(Control owner) : CefDisplayHandler
{
    protected override bool OnCursorChange(
        CefBrowser browser,
        IntPtr cursorHandle,
        CefCursorType type,
        CefCursorInfo customCursorInfo)
    {
        owner.DefaultCursorShape = MapCursor(type);

        // False: we only recorded the shape, CEF still owns the platform cursor.
        return false;
    }

    private static Control.CursorShape MapCursor(CefCursorType type)
    {
        return type switch
        {
            CefCursorType.IBeam or CefCursorType.VerticalText => Control.CursorShape.IBeam,
            CefCursorType.Hand => Control.CursorShape.Hand,
            CefCursorType.Cross or CefCursorType.Cell => Control.CursorShape.Crosshair,
            CefCursorType.Wait or CefCursorType.Progress => Control.CursorShape.Progress,
            CefCursorType.Move or CefCursorType.MiddlePanning => Control.CursorShape.Move,
            CefCursorType.NoDrop or CefCursorType.NotAllowed => Control.CursorShape.NotAllowed,
            CefCursorType.NorthSouthResize or CefCursorType.RowResize => Control.CursorShape.VResize,
            CefCursorType.EastWestResize or CefCursorType.ColumnResize => Control.CursorShape.HResize,
            CefCursorType.NorthWestSouthEastResize => Control.CursorShape.NWSEResize,
            CefCursorType.NorthEastSouthWestResize => Control.CursorShape.NESWResize,
            CefCursorType.NorthResize => Control.CursorShape.NResize,
            CefCursorType.SouthResize => Control.CursorShape.SResize,
            CefCursorType.EastResize => Control.CursorShape.EResize,
            CefCursorType.WestResize => Control.CursorShape.WResize,
            CefCursorType.NorthEastResize => Control.CursorShape.NEResize,
            CefCursorType.NorthWestResize => Control.CursorShape.NWResize,
            CefCursorType.SouthEastResize => Control.CursorShape.SEResize,
            CefCursorType.SouthWestResize => Control.CursorShape.SWResize,
            _ => Control.CursorShape.Arrow,
        };
    }
}
