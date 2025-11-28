using Eaship.Models;
using PdfSharp.Drawing;
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

        Directory.CreateDirectory(folder);

        string fileName = $"contract_{contract.ContractId}.pdf";
        string filePath = Path.Combine(folder, fileName);

        PdfDocument doc = new PdfDocument();
        doc.Info.Title = "Contract Document";

        PdfPage page = doc.AddPage();
        XGraphics gfx = XGraphics.FromPdfPage(page);

        XFont titleFont = new XFont("DefaultFont", 20, XFontStyleEx.Bold);
        XFont textFont = new XFont("DefaultFont", 12, XFontStyleEx.Regular);




        // HEADER
        gfx.DrawString(
            "EaShip Contract Agreement",
            titleFont,
            XBrushes.Black,
            new XRect(
                XUnit.FromPoint(0).Point,
                XUnit.FromPoint(20).Point,
                XUnit.FromPoint(page.Width.Point).Point,
                XUnit.FromPoint(40).Point
            ),
            XStringFormats.TopCenter
        );

        double y = 80;

        void WriteLine(string text)
        {
            gfx.DrawString(
                text,
                textFont,
                XBrushes.Black,
                new XRect(
                    40,
                    y,
                    page.Width - 80,
                    20
                ),
                XStringFormats.TopLeft
            );


            y += 25;
        }

        // DATA
        WriteLine($"Contract ID   : {contract.ContractId}");
        WriteLine($"Booking ID    : {booking.BookingId}");
        WriteLine($"Renter Name   : {renter.Nama}");
        WriteLine($"Address       : {renter.Address}");
        WriteLine($"Cargo         : {booking.CargoDesc}");
        WriteLine($"Route         : {booking.OriginPort} → {booking.DestinationPort}");
        WriteLine($"Duration      : {booking.DurationDays} days");
        WriteLine($"Start Date    : {booking.StartDate:dd MMM yyyy}");
        WriteLine($"Created At    : {contract.CreatedAt:dd MMM yyyy HH:mm}");
        WriteLine("");

        WriteLine("This rental agreement is created between:");
        WriteLine(" - EaShip Company (Service Provider)");
        WriteLine($" - {renter.Nama} (Client)");
        WriteLine("");

        WriteLine("The client agrees to the following conditions:");
        WriteLine("1. Payment must be made according to agreement.");
        WriteLine("2. Cancellation may incur additional costs.");
        WriteLine("3. Both parties must comply with safety rules.");

        doc.Save(filePath);
        return filePath;
    }
}
