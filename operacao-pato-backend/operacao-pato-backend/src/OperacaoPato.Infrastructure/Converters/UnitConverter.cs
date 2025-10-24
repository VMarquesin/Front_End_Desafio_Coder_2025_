using System;

namespace OperacaoPato.Infrastructure.Converters
{
    public static class UnitConverter
    {
        public static double ConvertFeetToCentimeters(double feet)
        {
            return feet * 30.48;
        }

        public static double ConvertPoundsToGrams(double pounds)
        {
            return pounds * 453.592;
        }

        public static double ConvertYardsToCentimeters(double yards)
        {
            return yards * 91.44;
        }

        public static double ConvertCentimetersToFeet(double centimeters)
        {
            return centimeters / 30.48;
        }

        public static double ConvertGramsToPounds(double grams)
        {
            return grams / 453.592;
        }

        public static double ConvertCentimetersToYards(double centimeters)
        {
            return centimeters / 91.44;
        }
    }
}