using Newtonsoft.Json;
using Quokka.ListItems;
using Quokka.PluginArch;
using System.IO;
using WinCopies.Util;

namespace Plugin_Workflows {
  /// <summary>
  /// The Workflows plugin
  /// </summary>
  public class Plugin_Workflows : Plugin {

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public override string PluggerName { get; set; } = "Workflows";

    private static Settings pluginSettings = new();
    internal static Settings PluginSettings { get => pluginSettings; set => pluginSettings = value; }


    /// <summary>
    /// Loads plugin settings
    /// </summary>
    public Plugin_Workflows() {
      string fileName = Environment.CurrentDirectory + "\\PlugBoard\\Plugin_Workflows\\Plugin\\settings.json";
      PluginSettings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(fileName))!;
    }


    private List<ListItem> createItems(string query) {
      List<ListItem> items = new List<ListItem>() { };
      foreach (List<string> workflow in PluginSettings.Workflows) {
        if (FuzzySearch.LD(workflow[0], query) < PluginSettings.FuzzySearchThreshold || workflow[0].Contains(query, StringComparison.OrdinalIgnoreCase)) {
          items.Add(new Workflow(workflow[0], workflow.Skip(1).ToList()));
        }
      }
      return items;
    }

    /// <summary>
    /// <inheritdoc/><br />
    /// Provides all workflow special commands on the AllWorkflowsCommand.<br />
    /// Provides the workflow on the workflow's command (first string in it's list)
    /// </summary>
    /// <param name="command"><inheritdoc/></param>
    /// <returns>the relevant workflow special commands</returns>
    public override List<ListItem> OnSpecialCommand(string command) {
      switch (command) {
        case var value when value == PluginSettings.AllWorkflowsCommand:
          List<ListItem> items = new List<ListItem>() { };
          foreach (List<string> workflow in PluginSettings.Workflows) {
            items.Add(new Workflow(workflow[0], workflow.Skip(1).ToList()));
          }
          return items;
        default:
          items = new List<ListItem>() { };
          foreach (List<string> workflow in PluginSettings.Workflows) {
            if (workflow[0] == command) {
              items.Add(new Workflow(workflow[0], workflow.Skip(1).ToList()));
              return items;
            }
          }
          return items;
      }
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <returns>The AllWorkflowsCommand and the first string in every list in the Workflows setting</returns>
    public override List<String> SpecialCommands() {
      List<String> Commands = new List<String>() { PluginSettings.AllWorkflowsCommand };
      foreach (List<string> workflow in PluginSettings.Workflows) {
        Commands.Add(workflow[0]);
      }
      return Commands;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="query"><inheritdoc/></param>
    /// <returns></returns>
    public override List<ListItem> OnQueryChange(string query) {
      return createItems(query);
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <returns>
    /// The Signifier from plugin settings
    /// </returns>
    public override List<string> CommandSignifiers() {
      return new List<string>() { PluginSettings.Signifier };
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="command">The Signifier (Since there is only 1 signifier for this plugin), followed by the query being searched for</param>
    /// <returns>A search item to open the search in the user's browser</returns>
    public override List<ListItem> OnSignifier(string command) {
      command = command.Substring(PluginSettings.Signifier.Length);
      return createItems(command);
    }

  }

}
