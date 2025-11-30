using Eaship.Models;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Fonts;
using PdfSharp.Pdf;
using System;
using System.IO;

public static class ContractPdfGenerator
{
    public static string GenerateContractPdf(Contract contract, Booking booking, RenterCompany renter)
    {
        GlobalFontSettings.FontResolver = new SimpleFontResolver();

        string folder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "EaShip",
            "Contracts"
        );
        Directory.CreateDirectory(folder);

        string fileName = $"contract_{contract.ContractId}.pdf";
        string filePath = Path.Combine(folder, fileName);

        PdfDocument doc = new PdfDocument();
        doc.Info.Title = "EaShip Contract Agreement";
        PdfPage page = doc.AddPage();

        XGraphics gfx = XGraphics.FromPdfPage(page);

        // Fonts
        XFont headerFont = new XFont("Arial", 18);
        XFont titleFont = new XFont("Arial", 14);
        XFont labelFont = new XFont("Arial", 11);
        XFont textFont = new XFont("Arial", 10);
        XFont smallFont = new XFont("Arial", 9);

        // Colors
        XColor primaryColor = XColor.FromArgb(0, 51, 102);
        XColor lightGray = XColor.FromArgb(240, 240, 240);

        double y = 20;
        double margin = 40;
        double pageWidth = page.Width.Point;

        // ===== HEADER SECTION =====
        // Draw header background
        gfx.DrawRectangle(new XSolidBrush(primaryColor), 0, 0, pageWidth, 60);

        gfx.DrawString(
            "EaShip Contract Agreement",
            headerFont,
            XBrushes.White,
            new XRect(margin, 15, pageWidth - (2 * margin), 40),
            XStringFormats.TopLeft
        );

        y = 80;

        // ===== CONTRACT INFO SECTION =====
        DrawSectionTitle("CONTRACT INFORMATION", gfx, margin, ref y, titleFont, primaryColor, pageWidth);

        DrawInfoRow("Contract ID", contract.ContractId.ToString(), gfx, margin, ref y, labelFont, textFont, pageWidth);
        DrawInfoRow("Booking ID", booking.BookingId.ToString(), gfx, margin, ref y, labelFont, textFont, pageWidth);
        DrawInfoRow("Issue Date", contract.CreatedAt.ToString("dd MMMM yyyy HH:mm"), gfx, margin, ref y, labelFont, textFont, pageWidth);

        y += 10;

        // ===== PARTIES SECTION =====
        DrawSectionTitle("PARTIES", gfx, margin, ref y, titleFont, primaryColor, pageWidth);

        gfx.DrawString("Service Provider:", labelFont, XBrushes.Black, margin, y);
        y += 18;
        gfx.DrawString("EaShip Company", textFont, XBrushes.Black, margin + 15, y);
        y += 15;

        gfx.DrawString("Client:", labelFont, XBrushes.Black, margin, y);
        y += 18;
        gfx.DrawString(renter.Nama, textFont, XBrushes.Black, margin + 15, y);
        gfx.DrawString(renter.Address, smallFont, XBrushes.Black, margin + 15, y + 13);
        y += 35;

        // ===== SHIPMENT DETAILS SECTION =====
        DrawSectionTitle("SHIPMENT DETAILS", gfx, margin, ref y, titleFont, primaryColor, pageWidth);

        DrawInfoRow("Cargo Description", booking.CargoDesc, gfx, margin, ref y, labelFont, textFont, pageWidth);
        DrawInfoRow("Origin Port", booking.OriginPort, gfx, margin, ref y, labelFont, textFont, pageWidth);
        DrawInfoRow("Destination Port", booking.DestinationPort, gfx, margin, ref y, labelFont, textFont, pageWidth);
        DrawInfoRow("Duration", $"{booking.DurationDays} days", gfx, margin, ref y, labelFont, textFont, pageWidth);
        DrawInfoRow("Start Date", booking.StartDate.ToString("dd MMMM yyyy"), gfx, margin, ref y, labelFont, textFont, pageWidth);

        y += 10;

        // ===== TERMS & CONDITIONS SECTION =====
        DrawSectionTitle("TERMS & CONDITIONS", gfx, margin, ref y, titleFont, primaryColor, pageWidth);

        DrawBulletPoint("Payment must be completed in full according to the agreed schedule.", gfx, margin, ref y, textFont, pageWidth);
        DrawBulletPoint("Client is responsible for accurate cargo information and documentation.", gfx, margin, ref y, textFont, pageWidth);
        DrawBulletPoint("Cancellation requests must be submitted at least 7 days in advance.", gfx, margin, ref y, textFont, pageWidth);
        DrawBulletPoint("Cancellations within 7 days will incur 50% of total charges.", gfx, margin, ref y, textFont, pageWidth);
        DrawBulletPoint("Both parties must comply with national maritime safety regulations.", gfx, margin, ref y, textFont, pageWidth);
        DrawBulletPoint("EaShip is not liable for delays due to weather or force majeure events.", gfx, margin, ref y, textFont, pageWidth);
        DrawBulletPoint("Insurance coverage is available upon request.", gfx, margin, ref y, textFont, pageWidth);

        y += 15;

        // ===== PAYMENT SECTION =====
        DrawSectionTitle("PAYMENT INFORMATION", gfx, margin, ref y, titleFont, primaryColor, pageWidth);

        // Payment highlight box
        gfx.DrawRectangle(new XSolidBrush(lightGray), margin, y, pageWidth - (2 * margin), 50);
        gfx.DrawString(
            "For payment inquiries and further details, please contact us:",
            labelFont,
            XBrushes.Black,
            new XRect(margin + 10, y + 8, pageWidth - (2 * margin) - 20, 15),
            XStringFormats.TopLeft
        );

        gfx.DrawString(
            " +32(0)7323620",
            new XFont("Arial", 12),
            new XSolidBrush(primaryColor),
            new XRect(margin + 10, y + 28, pageWidth - (2 * margin) - 20, 15),
            XStringFormats.TopLeft
        );

        y += 65;

        // ===== SIGNATURES SECTION =====
        y += 10;
        DrawSectionTitle("SIGNATURES", gfx, margin, ref y, titleFont, primaryColor, pageWidth);

        double sig1X = margin;
        double sig2X = (pageWidth / 2) + 20;

        gfx.DrawLine(XPens.Black, sig1X, y + 50, sig1X + 100, y + 50);
        gfx.DrawString("EaShip Authorized", smallFont, XBrushes.Black, sig1X, y + 55);
        gfx.DrawString("Date: _____________", smallFont, XBrushes.Black, sig1X, y + 68);

        gfx.DrawLine(XPens.Black, sig2X, y + 50, sig2X + 100, y + 50);
        gfx.DrawString("Client Signature", smallFont, XBrushes.Black, sig2X, y + 55);
        gfx.DrawString("Date: _____________", smallFont, XBrushes.Black, sig2X, y + 68);

        // ===== FOOTER =====
        gfx.DrawLine(XPens.Gray, margin, page.Height.Point - 30, pageWidth - margin, page.Height.Point - 30);
        gfx.DrawString(
            "This document is confidential and legally binding. © 2024 EaShip Company. All rights reserved.",
            smallFont,
            XBrushes.Gray,
            new XRect(margin, page.Height.Point - 25, pageWidth - (2 * margin), 20),
            XStringFormats.BottomCenter
        );

        doc.Save(filePath);
        return filePath;
    }

    private static void DrawSectionTitle(string title, XGraphics gfx, double x, ref double y, XFont font, XColor color, double pageWidth)
    {
        // Section Title (clean)
        gfx.DrawString(title, font, new XSolidBrush(color), x, y);

        // Underline instead of rectangle
        gfx.DrawLine(new XPen(color, 1.5), x, y + 18, x + 250, y + 18);

        y += 30;

    }

    private static void DrawInfoRow(string label, string value, XGraphics gfx, double x, ref double y, XFont labelFont, XFont textFont, double pageWidth)
    {
        gfx.DrawString(label + ":", labelFont, XBrushes.Black, x, y);
        gfx.DrawString(value, textFont, XBrushes.Black, x + 150, y);
        y += 18;
    }

    private static void DrawBulletPoint(string text, XGraphics gfx, double x, ref double y, XFont font, double pageWidth)
    {
        gfx.DrawString("•", font, XBrushes.Black, x, y);

        XTextFormatter tf = new XTextFormatter(gfx);
        tf.DrawString(
            text,
            font,
            XBrushes.Black,
            new XRect(x + 15, y, pageWidth - (x + 40), 40),
            XStringFormats.TopLeft
        );

        y += 22;
    }
}