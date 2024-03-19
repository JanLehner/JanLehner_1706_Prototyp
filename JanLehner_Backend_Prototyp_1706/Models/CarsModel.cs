using System.Data.SqlClient;

namespace JanLehner_Backend_Prototyp_1706.Models
{
    public class CarsModel
    {
        public static async Task<string> EnterACar(string imagePath)
        {
            try
            {
                string numberPlate = await PlateRecognizerModel.RecognizePlate(imagePath);
                CarModel carRecord = getCarRecordFromDB(numberPlate);
                if (carRecord.NumberPlate == null || carRecord.NumberPlate == "")
                {
                    createCarRecord(numberPlate);
                    return "Schranke öffnen";
                }
                else
                {
                    if (carRecord.IsParked == false)
                    {
                        updateParkStatus(carRecord);
                        return "Schranke öffnen";
                    }
                    else
                    {
                        return "Schranke geschlossen halten";
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error while trying to enter a car {e.Message}");
                return "Schranke geschlossen halten";
            }
        }

        public static string PayForACar(string numberPlate)
        {
            CarModel carRecord = getCarRecordFromDB(numberPlate);
            if (carRecord.NumberPlate == null || carRecord.NumberPlate == "" || carRecord.IsParked == false)
            {
                return "Kein Eintrag gefunden";
            }
            else
            {
                DateTime currentTime = DateTime.Now;
                TimeSpan difference = currentTime.Subtract(carRecord.PaidUntil);

                double minutesDifference = difference.TotalMinutes;

                if (minutesDifference < 0)
                {
                    return "Bereits bezahlt";
                }
                else
                {
                    string returnString = "";
                    switch (minutesDifference)
                    {
                        case < 10:
                            returnString = "0";
                            break;
                        case < 15:
                            returnString = "1.0";
                            break;
                        case < 30:
                            returnString = "2.0";
                            break;
                        case < 60:
                            returnString = "4.0";
                            break;
                        default:
                            returnString = "10.0";
                            break;
                    }
                    extendPayedUntil(carRecord);
                    return returnString;
                }
            }
        }

        public static async Task<string> ExitACar(string imagePath)
        {
            try
            {
                string numberPlate = await PlateRecognizerModel.RecognizePlate(imagePath);
                CarModel carRecord = getCarRecordFromDB(numberPlate);

                if (carRecord.NumberPlate == null || carRecord.NumberPlate == "")
                {
                    return "Schranke geschlossen halten";
                }
                else
                {
                    if(DateTime.Now < carRecord.PaidUntil && carRecord.IsParked == true)
                    {
                        updateParkStatus(carRecord);
                        return "Schranke öffnen";
                    }
                    else
                    {
                        return "Schranke geschlossen halten";
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error while trying to enter a car {e.Message}");
                return "Schranke geschlossen halten";
            }
        }

        private static CarModel getCarRecordFromDB(string numberPlate)
        {
            CarModel carRecord = new CarModel();
            const string query = @"SELECT * FROM Cars WHERE numberPlate = @searchNumberPlate;";

            try
            {
                using (SqlConnection conn = new SqlConnection(Database.CONNECTION_STRING))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@searchNumberPlate", numberPlate);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                CarModel recordOfCar = new CarModel
                                {
                                    CarID = (int)reader["carID"],
                                    NumberPlate = (string)reader["numberPlate"],
                                    IsParked = (bool)reader["isParked"],
                                    PaidUntil = (DateTime)reader["paidUntil"],
                                };

                                carRecord = recordOfCar;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error while trying to fetch data from the database {e.Message}");
            }

            return carRecord;
        }

        private static void createCarRecord(string numberPlate)
        {
            const string query = "INSERT INTO Cars (numberPlate, isParked, paidUntil) VALUES (@NumberPlate, @IsParked, @PaidUntil)";
            try
            {
                using (SqlConnection conn = new SqlConnection(Database.CONNECTION_STRING))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@NumberPlate", numberPlate);
                        cmd.Parameters.AddWithValue("@IsParked", true);
                        cmd.Parameters.AddWithValue("@PaidUntil", DateTime.Now.AddMinutes(5));

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error while trying to create a data record {e.Message}");
            }

        }

        private static void extendPayedUntil(CarModel car)
        {
            const string updateQuery = "UPDATE Cars SET paidUntil = @PaidUntil WHERE numberPlate = @NumberPlate";

            try
            {
                using (SqlConnection conn = new SqlConnection(Database.CONNECTION_STRING))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@PaidUntil", DateTime.Now.AddMinutes(10)); /*Hier evtl. nach Anforderungen anpassen*/
                        cmd.Parameters.AddWithValue("@NumberPlate", car.NumberPlate);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error while trying to update PaidUntil: {e.Message}");
            }
        }

        private static void updateParkStatus(CarModel car)
        {
            const string updateQuery = "UPDATE Cars SET isParked = @IsParked WHERE numberPlate = @NumberPlate";

            try
            {
                using (SqlConnection conn = new SqlConnection(Database.CONNECTION_STRING))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@IsParked", !car.IsParked);
                        cmd.Parameters.AddWithValue("@NumberPlate", car.NumberPlate);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error while trying to update isParked: {e.Message}");
            }
        }
    }
}

