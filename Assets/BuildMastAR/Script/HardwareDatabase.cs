using System.Collections.Generic;
using UnityEngine;

// Our data structure for a single hardware component
[System.Serializable]
public class HardwareItem
{
    public string partName;
    public string partSize;
    public int quantity;
    public string imageName; // We will use this to load the icon dynamically later
}

public static class HardwareDatabase
{
    // Pass the furniture string (e.g., "Sofa") and the step index (0 for Step 1)
    public static List<HardwareItem> GetHardwareForStep(string furnitureName, int stepIndex)
    {
        List<HardwareItem> parts = new List<HardwareItem>();

        switch (furnitureName)
        {
            // ==========================================
            // 1. STANDARD SOFA
            // ==========================================
            case "Sofa":
                switch (stepIndex)
                {
                    case 0: // Base
                        parts.Add(new HardwareItem { partName = "Connecting Bolts", partSize = "M8 40mm", quantity = 4, imageName = "bolt_icon" });
                        parts.Add(new HardwareItem { partName = "Flat Washers", partSize = "M8", quantity = 4, imageName = "washer_icon" });
                        break;
                    case 1: // Legs (4x)
                        parts.Add(new HardwareItem { partName = "Hex Head Bolts", partSize = "M8 60mm", quantity = 4, imageName = "hex_bolt_icon" });
                        parts.Add(new HardwareItem { partName = "Spring Washers", partSize = "M8", quantity = 4, imageName = "spring_washer_icon" });
                        break;
                    case 2: // Rear Support
                        parts.Add(new HardwareItem { partName = "Cam Lock Nuts", partSize = "15mm", quantity = 4, imageName = "cam_nut_icon" });
                        parts.Add(new HardwareItem { partName = "Cam Bolts", partSize = "40mm", quantity = 4, imageName = "cam_bolt_icon" });
                        break;
                    case 3: // Arm Rests
                        parts.Add(new HardwareItem { partName = "Allen Head Bolts", partSize = "M6 50mm", quantity = 6, imageName = "allen_bolt_icon" });
                        parts.Add(new HardwareItem { partName = "Wooden Dowels", partSize = "8x30mm", quantity = 4, imageName = "dowel_icon" });
                        break;
                    case 4: // Cushions
                        // Cushions usually just slide into place or use pre-attached velcro. No hardware.
                        break;
                }
                break;

            // ==========================================
            // 2. LARGE SOFA
            // ==========================================
            case "LargeSofa":
                switch (stepIndex)
                {
                    case 0: // Base & Rear Support
                        parts.Add(new HardwareItem { partName = "Connecting Bolts", partSize = "M8 50mm", quantity = 6, imageName = "bolt_icon" });
                        parts.Add(new HardwareItem { partName = "Cam Lock Nuts", partSize = "15mm", quantity = 4, imageName = "cam_nut_icon" });
                        break;
                    case 1: // Legs (4x)
                        parts.Add(new HardwareItem { partName = "Hex Head Bolts", partSize = "M8 60mm", quantity = 4, imageName = "hex_bolt_icon" });
                        parts.Add(new HardwareItem { partName = "Flat Washers", partSize = "M8", quantity = 4, imageName = "washer_icon" });
                        break;
                    case 2: // Arm Rests
                        parts.Add(new HardwareItem { partName = "Allen Head Bolts", partSize = "M8 50mm", quantity = 8, imageName = "allen_bolt_icon" });
                        parts.Add(new HardwareItem { partName = "Wooden Dowels", partSize = "8x30mm", quantity = 4, imageName = "dowel_icon" });
                        break;
                    case 3: // Back Cushions
                    case 4: // Seat Cushions
                        break;
                }
                break;

            // ==========================================
            // 3. TEAPOY (COFFEE TABLE)
            // ==========================================
            case "Teapoy":
                switch (stepIndex)
                {
                    case 0: // Utility Tray
                        parts.Add(new HardwareItem { partName = "Cam Bolts", partSize = "35mm", quantity = 4, imageName = "cam_bolt_icon" });
                        parts.Add(new HardwareItem { partName = "Wooden Dowels", partSize = "6x30mm", quantity = 4, imageName = "dowel_icon" });
                        break;
                    case 1: // Legs (4x)
                        parts.Add(new HardwareItem { partName = "Allen Head Bolts", partSize = "M6 45mm", quantity = 8, imageName = "allen_bolt_icon" });
                        parts.Add(new HardwareItem { partName = "Spring Washers", partSize = "M6", quantity = 8, imageName = "spring_washer_icon" });
                        break;
                    case 2: // Head (Tabletop)
                        parts.Add(new HardwareItem { partName = "Cam Lock Nuts", partSize = "15mm", quantity = 4, imageName = "cam_nut_icon" });
                        parts.Add(new HardwareItem { partName = "L-Brackets", partSize = "Small", quantity = 4, imageName = "bracket_icon" });
                        parts.Add(new HardwareItem { partName = "Wood Screws", partSize = "4x15mm", quantity = 16, imageName = "screw_icon" });
                        break;
                }
                break;

            // ==========================================
            // 4. CHAIR
            // ==========================================
            case "Chair":
                switch (stepIndex)
                {
                    case 0: // Main Frame
                        // Often serves as the starting anchor.
                        break;
                    case 1: // Rails (Backrest spindles)
                        parts.Add(new HardwareItem { partName = "Wooden Dowels", partSize = "6x20mm", quantity = 10, imageName = "dowel_icon" });
                        break;
                    case 2: // Support Frame (Seat Apron)
                        parts.Add(new HardwareItem { partName = "Wood Screws", partSize = "4x40mm", quantity = 8, imageName = "screw_icon" });
                        parts.Add(new HardwareItem { partName = "Wooden Dowels", partSize = "8x30mm", quantity = 8, imageName = "dowel_icon" });
                        break;
                    case 3: // Front Legs
                        parts.Add(new HardwareItem { partName = "Hex Head Bolts", partSize = "M6 50mm", quantity = 4, imageName = "hex_bolt_icon" });
                        parts.Add(new HardwareItem { partName = "Spring Washers", partSize = "M6", quantity = 4, imageName = "spring_washer_icon" });
                        break;
                    case 4: // Cushion
                        parts.Add(new HardwareItem { partName = "Wood Screws", partSize = "4x35mm", quantity = 4, imageName = "screw_icon" }); // Driven up from the bottom
                        break;
                }
                break;

            // ==========================================
            // 5. CRIB
            // ==========================================
            case "Crib":
                switch (stepIndex)
                {
                    case 0: // Bottom Frame & Base Frames
                        parts.Add(new HardwareItem { partName = "Barrel Nuts", partSize = "M6", quantity = 12, imageName = "barrel_nut_icon" });
                        parts.Add(new HardwareItem { partName = "Allen Head Bolts", partSize = "M6 40mm", quantity = 12, imageName = "allen_bolt_icon" });
                        break;
                    case 1: // Corner Frames
                        parts.Add(new HardwareItem { partName = "Connecting Bolts", partSize = "M6 50mm", quantity = 8, imageName = "bolt_icon" });
                        break;
                    case 2: // Back Frame Slats
                    case 3: // Right Frame Slats
                    case 4: // Left Frame Slats
                    case 5: // Front Frame Slats
                        // Slats almost always snap together using dowels during assembly before capping
                        parts.Add(new HardwareItem { partName = "Wooden Dowels", partSize = "8x30mm", quantity = 8, imageName = "dowel_icon" });
                        break;
                    case 6: // Top Frames
                        parts.Add(new HardwareItem { partName = "Allen Head Bolts", partSize = "M6 60mm", quantity = 8, imageName = "allen_bolt_icon" });
                        parts.Add(new HardwareItem { partName = "Cam Lock Nuts", partSize = "15mm", quantity = 8, imageName = "cam_nut_icon" });
                        break;
                    case 7: // Mattress
                        break;
                }
                break;
        }

        return parts;
    }
}