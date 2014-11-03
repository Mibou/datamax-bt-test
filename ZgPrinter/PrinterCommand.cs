using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using ZgLib;

namespace ZgPrinter
{
    /// <summary>
    /// This super class contains the skeleton to build a Datamax E-Class Mark III command over it
    /// </summary>
    class PrinterCommand
    {
        // Print format
        protected delegate string print_format(string input);
        protected print_format boldstring = x => "FB+\n" + x + "FB-\n";
        protected string formatted_boldstring = "FB+\n";
        protected string formatted_normalstring = "FB-\n";

        // Print commands
        protected string stop = (char)1 + "C\r";
        protected string toggle_pause = (char)1 + "B\r";
        protected string status = (char)1 + "F\r";
        protected string send_quantity = (char)1 + "e\r";

        // Labels formating
        protected string label_width = (char)1 + "KW158\r";
        protected string label_start = (char)2 + "L\r";
        protected string label_end = "E\r";

        // Other formating
        protected string font_symbol_set = (char)2 + "ySW1\r";
        protected string print_dot_size = "D11\r";
        protected string print_zero_conversion = "z\r";
        protected string print_format_attribute_transp = "A2\r";
        protected string print_format_attribute_xor = "A1\r";
        protected string print_row_offset = "R0000\r";

        // Printer options
        protected string print_backfeed_speed = "pK\r";
        protected string print_peel = "PE\r";
        protected string print_heat = "H30\r";
        protected string print_speed = "PK\r";

        // Stored schema
        protected string schema;

        // Final bytes buffer
        protected byte[] buffer = null;

        Encoding encoding = Encoding.GetEncoding("windows-1252");

        /// <summary>
        /// Converts string schema to read-to-send bytes buffer
        /// </summary>
        /// <returns>Bytes buffer for the printer</returns>
        public byte[] get_buffer()
        {
            // Encoding for € character
            if (buffer == null)
                buffer = encoding.GetBytes(this.schema);

            return buffer;
        }
    }
}
