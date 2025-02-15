namespace Plugin_Workflows {

  /// <summary>
  /// All plugin specific settings
  /// </summary>
  public class Settings {
    /// <summary>
    /// The command signifier used to obtain a search item (defaults to "| ")
    ///</summary>
    public string Signifier { get; set; } = "| ";
    /// <summary>
    ///   The threshold for when to consider a workflow
    ///   command is similar enough to the query for it to be
    ///   displayed (defaults to 15). Currently uses the
    ///   Levenshtein distance; the larger the number, the
    ///   bigger the difference (being matched).
    /// </summary>
    public int FuzzySearchThreshold { get; set; } = 15;
    /// <summary>
    /// The command to list all Workflows (defaults to "All Workflows")
    /// </summary>
    public string AllWorkflowsCommand { get; set; } = "All Workflows";
    /// <summary>
    /// The workflows; The first string in each list is the workflow's command. The strings that follow are the commands that get executed by that workflow (in that order) (defaults to empty).
    /// </summary>
    public List<List<string>> Workflows { get; set; } = new List<List<string>>();

  }

}
