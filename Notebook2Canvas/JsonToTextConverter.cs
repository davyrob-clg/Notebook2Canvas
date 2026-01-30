using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Windows.Forms.VisualStyles;

public class JsonToTextConverter
{
    /// <summary>
    /// Converts a JSON file into a plain text file by extracting all string values.
    /// </summary>
    public String Convert(string jsonFilePath, string outputTextFilePath)
    {
        if (!File.Exists(jsonFilePath))
            throw new FileNotFoundException("JSON file not found.", jsonFilePath);

        string jsonContent = File.ReadAllText(jsonFilePath);

        JsonNode root = JsonNode.Parse(jsonContent);
        StringBuilder sb = new StringBuilder();
        StringBuilder sbQuestions = new StringBuilder();

        //ExtractText(root, sb);

        ExtractQuestion(root, sbQuestions);

        File.WriteAllText(outputTextFilePath, sbQuestions.ToString());

        return sbQuestions.ToString();
    }

    /// <summary>
    /// Extract JSON from a notebook format and then create a file that can be converted by texttoqti - python code.
    /// This works in a particular format - since the questions exported are known
    /// </summary>
    private void ExtractQuestion(JsonNode node, StringBuilder sb)
    {
        int QuestionCount = 0;

        // The first step is to get to the array of questions
        JsonArray qArray = node.AsArray();

        int nQuestions = qArray.Count;
        Console.WriteLine("Number of questions: " + nQuestions);


        foreach (JsonObject item in qArray)
        {

            JsonObject questionItem = item;

            Console.WriteLine("Name: " + item.ToString());

            String question = questionItem["question"].ToString();
            String hint = questionItem["hint"].ToString();

            JsonArray questions = item["answerOptions"].AsArray();

            QuestionCount++;

            sb.AppendLine("Title: Question " + QuestionCount);
            // TODO can make this a global from the UI
            sb.AppendLine("Points: 1");
            int qNumber = 1;
            sb.AppendLine(qNumber + ". " + question);
            sb.AppendLine("... " + hint);

            // Ascii here 
            int num = 65;
            char c = 'a';

            foreach (var q in questions)
            {


                if (q is JsonObject jObj)
                {

                    Boolean isCorrect = jObj["isCorrect"].ToString().ToLower() == "true";
                    String rational = jObj["rationale"].ToString();
                    String optionText = jObj["text"].ToString();

                    if (isCorrect)
                    {
                        sb.AppendLine("*" + c + ") " + optionText);
                        sb.AppendLine("... " + rational);
                    }
                    else
                    {
                        sb.AppendLine(c + ") " + optionText);
                        sb.AppendLine(string.Format("... {0}", rational));
                    }

                    c++;
                }



                //bool isCorrect = q["isCorrect"].toBoolean();
                //bool isCorrect = q["isCorrect"].toBoolean();

            }




        }
    }
    

    /// <summary>
    /// Recursively extracts all string values from a JSON node.
    /// </summary>
    private void ExtractText(JsonNode node, StringBuilder sb)
    {
        switch (node)
        {
            case JsonValue value:
                if (value.TryGetValue<string>(out var str))
                {
                    sb.AppendLine(str);
                }
                break;

            case JsonObject obj:
                foreach (var property in obj)
                {
                    ExtractText(property.Value, sb);
                }
                break;

            case JsonArray array:
                foreach (var item in array)
                {
                    ExtractText(item, sb);
                }
                break;
        }
    }
}
