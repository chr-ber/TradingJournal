namespace TradingJournal.Application.Common.Models;

// used to parse validation errors on client and display them to the user
public class ValidationErrorResponse
{
   [JsonPropertyName("title")]
   public string Title { get; set; }

   [JsonPropertyName("status")]
   public int Status { get; set; }

   [JsonPropertyName("errors")]
   public Dictionary<string, List<string>> Errors { get; set; }

   public override string ToString()
   {
      var response = new StringBuilder();

      foreach (var errorType in Errors)
      {
         foreach (var errorMessage in errorType.Value)
         {
            response.Append($"{errorMessage}");
         }

         // filter errors that are related to multiple fields
         if (string.IsNullOrWhiteSpace(errorType.Key) is false)
         {
            // add property name at the end of the message
            response.AppendLine($" ({errorType.Key})");
         }
      }
      return response.ToString();
   }
}

