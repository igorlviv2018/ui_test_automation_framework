using Taf.UI.Core.Configuration;
using Taf.UI.Core.Enums;
using Taf.UI.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace Taf.UI.Core.Helpers
{
    public class StaticTestDataHelper
    {
        private readonly DataHelper dataHelper = new DataHelper();

        private readonly TestConfiguration testConfig = new TestConfiguration();

        private List<TafEmArticleElement> ElementsInArticle { get; set; } = new List<TafEmArticleElement>();

        //-----------Custom articles------------

        //001: Custom article with nested collapses (text, image, video, step-by-step)
        public List<TafEmArticleElement> TestDataArticle001()
        {
            ElementsInArticle.Clear();

            new TafEmArticleElement().CreateRoot(ElementsInArticle, "root")
                .AddAccordion(ElementsInArticle, "Accordion 1", "topLevel")
                    .AddCollapse(ElementsInArticle, "Collapse 1")
                        .AddText(ElementsInArticle, "Text in collapse 1")
                             .AddTextLinks("google", "https://www.google.com")
                        .AddVideo(ElementsInArticle, "https://www.youtube.com/watch?v=ZGn4v1cbt0A", "Video")
                        .AddButtonsBlock(ElementsInArticle, "buttons")
                            .AddButton(ProcessButtonClickAction.Link, "Link", "https://www.google.com.ua")
                            .AddButton(ProcessButtonClickAction.Link, "Link2", "https://www.google.com.ua")
                        .AddImage(ElementsInArticle, "image2", GetImagePath("image_01.jpg"))
                        .AddStepByStep(ElementsInArticle, "Step-by-step")
                            .AddStep("Step 1", GetImagePath("image_01.jpg"))
                            .AddStep("Step 2", GetImagePath("image_02.jpg"))
                        .AddAccordion(ElementsInArticle)
                            .AddCollapse(ElementsInArticle, "Collapse 1 in nested accord")
                                .AddText(ElementsInArticle)
                            .AddCollapse(ElementsInArticle, "Collapse 2 in nested accord")
                                .AddText(ElementsInArticle)
                    .AddCollapse(ElementsInArticle, "Collapse 2", "topLevel")
                    .AddCollapse(ElementsInArticle, "Collapse 3", "topLevel")
                        .AddText(ElementsInArticle, "Text")

                .AddText(ElementsInArticle, "Text", "description", "root")
                    .AddTextLinks("google", "https://www.google.com", "link2", "https://www.google.com.ua")

                .AddAccordion(ElementsInArticle, "Accordion 2", "", true)
                    .AddCollapse(ElementsInArticle, "Collapse 1 (in accord 2)")
                        .AddImage(ElementsInArticle, "image2", GetImagePath("image_01.jpg"));

            return ElementsInArticle;
        }

        //002: Custom article with step-by-step in a collapse
        public List<TafEmArticleElement> TestDataArticle002()
        {
            ElementsInArticle.Clear();

            new TafEmArticleElement().CreateRoot(ElementsInArticle, "root")
                .AddAccordion(ElementsInArticle, "Accordion 1", "topLevel")
                    .AddCollapse(ElementsInArticle, "Collapse 1")
                        .AddText(ElementsInArticle, "Text in collapse 1")
                             .AddTextLinks("google", "https://www.google.com")
                    .AddCollapse(ElementsInArticle, "Collapse 2", "topLevel")
                        .AddStepByStep(ElementsInArticle, "Step-by-step")
                            .AddStep("Step 1", GetImagePath("image_01.jpg"))
                            .AddStep("Step 2", GetImagePath("image_02.jpg"))
                            .AddStep("Step 3", GetImagePath("image_06.png"), "google", "https://www.google.com.ua")
                    .AddCollapse(ElementsInArticle, "Collapse 3", "topLevel")
                        .AddText(ElementsInArticle, "Text");

            return ElementsInArticle;
        }

        //464: image, video, step-by-step in article without collapses
        public List<TafEmArticleElement> TestDataArticle464()
        {
            ElementsInArticle.Clear();

            new TafEmArticleElement().CreateRoot(ElementsInArticle, "root")
                .AddText(ElementsInArticle, "Text", "desc", "root")
                    .AddTextLinks("google", "https://www.google.com", "link2", "https://www.google.com.ua")
                .AddVideo(ElementsInArticle, "https://www.youtube.com/watch?v=ZGn4v1cbt0A", "Video", "root")
                .AddButtonsBlock(ElementsInArticle, "Buttons", "root")
                    .AddButton(ProcessButtonClickAction.Link, "Link", "https://www.google.com.ua")
                    .AddButton(ProcessButtonClickAction.Link, "Link2", "https://www.google.com.ua")
                .AddImage(ElementsInArticle, "Image", GetImagePath("image_01.jpg"), "root")
                .AddStepByStep(ElementsInArticle, "Step-by-step", "description", "root")
                    .AddStep("Step 1", GetImagePath("image_01.jpg"))
                    .AddStep("Step 2", GetImagePath("image_02.jpg"), "google", "https://www.google.com.ua")
                    .AddStep("Step 3", GetImagePath("image_06.png"));

            return ElementsInArticle;
        }

        //2036: simple article for tests of article operations (e.g. article archiving, restoring)
        public List<TafEmArticleElement> TestDataArticle2036()
        {
            ElementsInArticle.Clear();

            new TafEmArticleElement().CreateRoot(ElementsInArticle, "root")
                .AddText(ElementsInArticle, "Text", "desc", "root");

            return ElementsInArticle;
        }

        //-----------Diagnostic flows------------

        //836: DF with Processes with buttons - no decisions
        public List<TafEmArticleElement> TestDataDF836()
        {
            ElementsInArticle.Clear();

            TafEmArticleElement root = CreateRoot();

            TafEmArticleElement topLevelProc = CreateProcess(root, "Process element (txt+btn)", "description")
                .AddText()
                .AddButtonsBlock()
                    .AddButton(ProcessButtonClickAction.NextStep, "Next element")
                    .AddButton(ProcessButtonClickAction.PreviousStep, "Previous element");

            TafEmArticleElement proc2 = CreateProcess(topLevelProc, "Process element (video+btn)", "description")
                .AddButtonsBlock()
                    .AddButton(ProcessButtonClickAction.NextStep, "Next element")
                    .AddButton(ProcessButtonClickAction.PreviousStep, "Previous element");

            CreateTerminator(proc2, "Terminator (table + btn)")
                .AddTable()
                .AddButtonsBlock()
                    .AddButton(ProcessButtonClickAction.Link, "Link", "https://www.google.com")
                    .AddButton(ProcessButtonClickAction.RestartFlow, "Restart flow");

            AddBranchesToDecisions(ElementsInArticle);

            return ElementsInArticle;
        }

        //871: DF with nested decisions (up to level 5)
        public List<TafEmArticleElement> TestDataDF871()
        {
            ElementsInArticle.Clear();

            TafEmArticleElement root = CreateRoot();

            TafEmArticleElement topLevelProc = CreateProcess(root, "Process", "Simple process") // first process
                .AddText();

            // level 1 decision
            TafEmArticleElement decision = CreateDecision(topLevelProc, "Level 1 decision", 
                "description");

            TafEmArticleElement yesBranch = AddBranch(decision, "Yes");

            CreateTerminator(yesBranch, "End");

            TafEmArticleElement noBranch = AddBranch(decision, "No (nested decision)");

            // level 2 decision
            TafEmArticleElement decisionL2 = CreateDecision(noBranch, "Level 2 decision",
                "description");

            TafEmArticleElement yesBranchInL2 = AddBranch(decisionL2, "Yes");

            CreateTerminator(yesBranchInL2, "End");

            TafEmArticleElement noBranchInL2 = AddBranch(decisionL2, "No (nested decision)");

            // level 3 decision
            TafEmArticleElement decisionL3 = CreateDecision(noBranchInL2, "Level 3 decision",
                "description");

            TafEmArticleElement yesBranchInL3 = AddBranch(decisionL3, "Yes");

            CreateTerminator(yesBranchInL3, "End");

            TafEmArticleElement noBranchInL3 = AddBranch(decisionL3, "No (nested decision)");

            // level 4 decision
            TafEmArticleElement decisionL4 = CreateDecision(noBranchInL3, "Level 4 decision",
                "description");

            TafEmArticleElement yesBranchInL4 = AddBranch(decisionL4, "Yes");

            CreateTerminator(yesBranchInL4, "End");

            TafEmArticleElement noBranchInL4 = AddBranch(decisionL4, "No (nested decision)");

            TafEmArticleElement maybeBranchInL4 = AddBranch(decisionL4, "Maybe");

            CreateTerminator(maybeBranchInL4, "End");

            // level 5 decision
            TafEmArticleElement decisionL5 = CreateDecision(noBranchInL4, "Level 5 decision",
                "description");

            TafEmArticleElement yesBranchInL5 = AddBranch(decisionL5, "Yes");

            CreateTerminator(yesBranchInL5, "End");

            TafEmArticleElement noBranchInL5 = AddBranch(decisionL5, "No");

            CreateTerminator(noBranchInL5, "End");

            AddBranchesToDecisions(ElementsInArticle);

            return ElementsInArticle;
        }

        //933: DF with decision with dropdown (one dropdown item with image) and Processes with buttons (Id=933)
        public List<TafEmArticleElement> TestDataDF933()
        { 
            //========================== to update =================================================
            ElementsInArticle.Clear();

            TafEmArticleElement root = CreateRoot();

            TafEmArticleElement topLevelProc = CreateProcess(root, "Process")
                .AddText();

            TafEmArticleElement decision = CreateDecision(topLevelProc, "question", "decision description", hasButtonView: false);

            TafEmArticleElement yesBranch = AddBranch(decision, "yes", GetImagePath("image_01.jpg"));

            CreateTerminator(yesBranch, "end");

            TafEmArticleElement noBranch = AddBranch(decision, "no");

            TafEmArticleElement procNumOneInNoBranch = CreateProcess(noBranch, "Process w/ text and buttons", "process description")
                .AddText()
                .AddButtonsBlock()
                    .AddButton(ProcessButtonClickAction.PreviousStep, "Previous step")
                    .AddButton(ProcessButtonClickAction.RestartFlow, "Restart flow")
                    .AddButton(ProcessButtonClickAction.NextStep, "Next step")
                    .AddButton(ProcessButtonClickAction.Link, "Link", "https://www.google.com.ua");

            TafEmArticleElement procNumTwoInNoBranch = CreateProcess(procNumOneInNoBranch, "Process w/ Previous step btn", "Process w/ Previous step btn")
                .AddButtonsBlock()
                    .AddButton(ProcessButtonClickAction.PreviousStep, "Previous step");

            TafEmArticleElement procNumThreeInNoBranch = CreateProcess(procNumTwoInNoBranch, "Process w/ Next step btn")
                .AddText()
                .AddButtonsBlock()
                    .AddButton(ProcessButtonClickAction.NextStep, "Next step");

            TafEmArticleElement decisionNested = CreateDecision(procNumThreeInNoBranch, "nested decision", "nested decision");

            TafEmArticleElement yesBranchInNested = AddBranch(decisionNested, "Yes (button with image)", GetImagePath("image_01.jpg"));

            CreateTerminator(yesBranchInNested, "end", "terminator");

            TafEmArticleElement noBranchInNested = AddBranch(decisionNested, "No", GetImagePath("image_02.jpg"));

            CreateTerminator(noBranchInNested, "end of No", "terminator");

            AddBranchesToDecisions(ElementsInArticle);

            return ElementsInArticle;
        }

        //1948: Images and video (also in decision)
        public List<TafEmArticleElement> TestDataDF1948()
        {
            ElementsInArticle.Clear();

            TafEmArticleElement root = CreateRoot();

            TafEmArticleElement procWithImageVideo = CreateProcess(root, "proc with image, video", "process")
                .AddImage(GetImagePath("image_01.jpg"))//GetImagePath("image_01.jpg")
                .AddVideo("https://www.youtube.com/watch?v=8GAvVuRrNVA");

            TafEmArticleElement decision = CreateDecision(procWithImageVideo, "decision", "decision");

            TafEmArticleElement yesBranch = AddBranch(decision, "yes");

            TafEmArticleElement procWithImageVideoInDecision = CreateProcess(yesBranch, "proc with image, video", "process")
                .AddImage(GetImagePath("image_01.jpg"))
                .AddVideo("https://www.youtube.com/watch?v=ZGn4v1cbt0A");

            CreateTerminator(procWithImageVideoInDecision, "end of yes", "terminator");

            TafEmArticleElement noBranch = AddBranch(decision, "no");

            CreateTerminator(noBranch, "end of no", "terminator");

            AddBranchesToDecisions(ElementsInArticle);

            return ElementsInArticle;
        }

        //1029: DF with decision containing 5 branches
        public List<TafEmArticleElement> TestDataDF1029()
        {
            ElementsInArticle.Clear();

            TafEmArticleElement root = CreateRoot();

            TafEmArticleElement decision = CreateDecision(root, "Select an option", "description");

            TafEmArticleElement branch1 = AddBranch(decision, "answer 1"); //branch 1

            CreateTerminator(branch1, "End of 1");

            TafEmArticleElement branch2 = AddBranch(decision, "answer 2");

            CreateTerminator(branch2, "End of 2");

            TafEmArticleElement branch3 = AddBranch(decision, "answer 3");

            CreateTerminator(branch3, "End of 3");

            TafEmArticleElement branch4 = AddBranch(decision, "answer 4");

            CreateTerminator(branch4, "End of 4");

            TafEmArticleElement branch5 = AddBranch(decision, "answer 5");

            CreateTerminator(branch5, "End of 5");

            AddBranchesToDecisions(ElementsInArticle);

            return ElementsInArticle;
        }

        //2720: DF with decision containing images in branches
        public List<TafEmArticleElement> TestDataDF2720()
        {
            ElementsInArticle.Clear();

            TafEmArticleElement root = CreateRoot();

            TafEmArticleElement decision = CreateDecision(root, "Decision with images in branches", "description");

            TafEmArticleElement branch1 = AddBranch(decision, "yes", GetImagePath("image_01.jpg")); //branch 1

            CreateTerminator(branch1, "terminator", "end of yes");

            TafEmArticleElement branch2 = AddBranch(decision, "no", GetImagePath("image_02.jpg"));

            CreateTerminator(branch2, "terminator", "end of no");

            AddBranchesToDecisions(ElementsInArticle);

            return ElementsInArticle;
        }

        //1048: DF with predefined process ouside decision and inside decision
        public List<TafEmArticleElement> TestDataDF1048()
        {
            ElementsInArticle.Clear();

            TafEmArticleElement root = CreateRoot();

            TafEmArticleElement predefProcessWithGuide = CreatePredefinedProcess("Restart your phone", UiContentType.Guide,
                   root, "Predefined process with guide", "description");

            TafEmArticleElement predefProcessWithArticle = CreatePredefinedProcess("article_006", UiContentType.Article,
                predefProcessWithGuide, "Predefined process with article", "description");

            TafEmArticleElement decision = CreateDecision(predefProcessWithArticle, "Select an option",
                "description");

            //branch 1
            TafEmArticleElement branch1 = AddBranch(decision, "Show predefined process with guide");

            TafEmArticleElement predefinedWithGuideInDecision = CreatePredefinedProcess("Restart your phone", UiContentType.Guide,
                   branch1, "Predefined process with guide", "description");

            CreateTerminator(predefinedWithGuideInDecision, "End");

            TafEmArticleElement branch2 = AddBranch(decision, "Show predefined process with article");

            TafEmArticleElement predefinedWithArticleInDecision = CreatePredefinedProcess("article_006", UiContentType.Article,
                branch2, "Predefined process with article", "description");

            CreateTerminator(predefinedWithArticleInDecision, "End");

            AddBranchesToDecisions(ElementsInArticle);

            return ElementsInArticle;
        }

        //950: DF with internal connector to decision
        public List<TafEmArticleElement> TestDataDF950()
        {
            ElementsInArticle.Clear();

            TafEmArticleElement root = CreateRoot();

            TafEmArticleElement decision = CreateDecision(root, "decision with internal connector", "description");

            TafEmArticleElement yesBranch = AddBranch(decision, "Yes");

            TafEmArticleElement noBranch = AddBranch(decision, "No");

            TafEmArticleElement decisionAsIntConnector = CreateDecision(noBranch, "decision to be used in internal connector");

            // decision branches with images
            TafEmArticleElement yesBranchInL2 = AddBranch(decisionAsIntConnector, "Yes", GetImagePath("image_01.jpg"));

            TafEmArticleElement noBranchInL2 = AddBranch(decisionAsIntConnector, "No", GetImagePath("image_02.jpg"));

            CreateTerminator(yesBranchInL2, "end");

            CreateTerminator(noBranchInL2, "end");

            CreateInternalConnector(pointTo: decisionAsIntConnector, parent: yesBranch);

            AddBranchesToDecisions(ElementsInArticle);

            return ElementsInArticle;
        }

        //937: DF with internal connector pointing to Terminator
        public List<TafEmArticleElement> TestDataDF937()
        {
            ElementsInArticle.Clear();

            TafEmArticleElement root = CreateRoot();

            TafEmArticleElement decision = CreateDecision(root, "decision level 1");

            // branches
            TafEmArticleElement branchOne = AddBranch(decision, "1 (with int connector to Terminator)");

            TafEmArticleElement branchTwo = AddBranch(decision, "2");

            TafEmArticleElement decisionL2 = CreateDecision(branchTwo, "decision level 2");

            TafEmArticleElement branchOneInL2 = AddBranch(decisionL2, "1");

            TafEmArticleElement branchTwoInL2 = AddBranch(decisionL2, "2");

            TafEmArticleElement terminatorInBranchOneInL2 = CreateTerminator(branchOneInL2, "end of 1");

            CreateTerminator(branchTwoInL2, "end of 2")
                .AddImage(GetImagePath("image_01.jpg"));

            CreateInternalConnector(pointTo: terminatorInBranchOneInL2, parent: branchOne);

            AddBranchesToDecisions(ElementsInArticle);

            return ElementsInArticle;
        }

        //936: DF with internal connector to process (no decisions in DF)
        public List<TafEmArticleElement> TestDataDF936()
        {
            ElementsInArticle.Clear();

            TafEmArticleElement root = CreateRoot();

            TafEmArticleElement process1 = CreateProcess(root, "Process #1", "description")
                .AddText();

            TafEmArticleElement process2 = CreateProcess(process1, "Process #2", "description")
                .AddText();

            TafEmArticleElement process3 = CreateProcess(process2, "Process #3", "description")
                .AddText();

            CreateInternalConnector(pointTo: process1, parent: process3);

            AddBranchesToDecisions(ElementsInArticle);

            return ElementsInArticle;
        }

        //1030: DF with external connector with buttons (in processes) and decision
        public List<TafEmArticleElement> TestDataDF1030(bool isAuthoring=false)
        {
            ElementsInArticle.Clear();

            TafEmArticleElement root = CreateRoot();

            TafEmArticleElement topLevelProc = CreateProcess(root, "Process with Next step btn")
                .AddButtonsBlock()
                    .AddButton(ProcessButtonClickAction.NextStep, "Next step");

            TafEmArticleElement decision = CreateDecision(topLevelProc, "Select option", "description");

            TafEmArticleElement branchOne = AddBranch(decision, "1"); // branch #1

            TafEmArticleElement procInOneBranch = CreateProcess(branchOne, "Process")
                .AddText();

            AddExternalConnector(TestExternalFlowDataId934, procInOneBranch,
                externalFlowTitle: "934: External flow with processes with buttons (first process with Prev step and Restart flow btn), decision",
                title: "External connector", "description", isAuthoring); // external flow

            TafEmArticleElement branchTwo = AddBranch(decision, "2");

            CreateTerminator(branchTwo, "End");

            TafEmArticleElement branchThree = AddBranch(decision, "3");

            CreateTerminator(branchThree, "End");

            AddBranchesToDecisions(ElementsInArticle);

            return ElementsInArticle;
        }

        //2045: DF with external connector with predefined process and step-by-step
        public List<TafEmArticleElement> TestDataDF2045(bool isAuthoring=false)
        {
            ElementsInArticle.Clear();

            TafEmArticleElement root = CreateRoot();

            TafEmArticleElement topLevelProc = CreateProcess(root, "process", "description")
                .AddImage(GetImagePath("image_01.jpg"));

            AddExternalConnector(TestExternalFlowDataId2043, topLevelProc,
                externalFlowTitle: "2043: External flow with predefined process and step-by-step",
                title: "External connector", "description", isAuthoring: isAuthoring); // external flow

            return ElementsInArticle;
        }

        //934: External flow with processes with buttons (first process with Prev step and Restart flow btn), decision
        public List<TafEmArticleElement> TestExternalFlowDataId934(bool isExternalFlow=false)
        {
            if (!isExternalFlow)
            {
                ElementsInArticle.Clear();
            }

            TafEmArticleElement root = CreateRoot();

            TafEmArticleElement topLevelProc = CreateProcess(root, "Process with Prev step and Restart flow btns", "description")
                .AddButtonsBlock()
                    .AddButton(ProcessButtonClickAction.PreviousStep, "Previous step")
                    .AddButton(ProcessButtonClickAction.RestartFlow, "Restart flow");

            // process #2
            TafEmArticleElement procTwo = CreateProcess(topLevelProc, "Process with text and Next step btn")
                .AddText()
                .AddButtonsBlock()
                    .AddButton(ProcessButtonClickAction.NextStep, "Next step");

            TafEmArticleElement decision = CreateDecision(procTwo, "yes/no?");

            TafEmArticleElement yesBranch = AddBranch(decision, "Yes");

            CreateTerminator(yesBranch, "end", "terminator");

            TafEmArticleElement noBranch = AddBranch(decision, "No");

            CreateTerminator(noBranch, "end", "terminator");

            if (isExternalFlow)
            {
                AddBranchesToDecisions(ElementsInArticle);
            }

            return ElementsInArticle;
        }

        //2043: External flow with predefined process and step-by-step
        public List<TafEmArticleElement> TestExternalFlowDataId2043(bool isExternalFlow = false)
        {
            if (!isExternalFlow)
            {
                ElementsInArticle.Clear();
            }

            TafEmArticleElement root = CreateRoot();

            TafEmArticleElement proc = CreateProcess(root, "proc with step-by-step", "process")
                .AddStepByStepBlock()
                    .AddStep("", imageFilePath: GetImagePath("image_01.jpg"))
                    .AddStep("", imageFilePath: GetImagePath("image_02.jpg"))
                    .AddStep("", imageFilePath: GetImagePath("image_06.png"));

            TafEmArticleElement predefProcessWithArticle = CreatePredefinedProcess("article_006", UiContentType.Article,
                proc, "Predefined process with article", "description");

            CreateTerminator(predefProcessWithArticle, "terminator", "terminator")
                .AddImage(GetImagePath("image_01.jpg"));

            return ElementsInArticle;
        }

        //1701: Process (top-level) with Next button moving to a decision and process in a branch moving to a decision
        public List<TafEmArticleElement> TestDataDF1701()
        {
            ElementsInArticle.Clear();

            TafEmArticleElement root = CreateRoot();

            TafEmArticleElement procWithNextStep = CreateProcess(root, "process with Next step button moving to decision", "description")
                .AddButtonsBlock()
                    .AddButton(ProcessButtonClickAction.NextStep, "Next step");

            TafEmArticleElement procWithoutButtons = CreateProcess(procWithNextStep, "process without buttons", "process")
                .AddText();

            TafEmArticleElement decision = CreateDecision(procWithoutButtons, "question", "decision description");

            TafEmArticleElement yesBranch = AddBranch(decision, "yes");

            TafEmArticleElement procWithoutButtonsInBranch = CreateProcess(yesBranch, "process without buttons", "process")
                .AddText();

            TafEmArticleElement procWithPrevStep = CreateProcess(procWithoutButtonsInBranch, "process with Previous step button moving to decision", "process")
                .AddButtonsBlock()
                    .AddButton(ProcessButtonClickAction.PreviousStep, "Previous step");

            CreateTerminator(procWithPrevStep, "end of yes", "terminator");

            TafEmArticleElement noBranch = AddBranch(decision, "no");

            CreateTerminator(noBranch, "end of No", "terminator");

            AddBranchesToDecisions(ElementsInArticle);

            return ElementsInArticle;
        }

        //1705: Terminator with Previous step button (no decisions)
        public List<TafEmArticleElement> TestDataDF1705()
        {
            ElementsInArticle.Clear();

            TafEmArticleElement root = CreateRoot();

            TafEmArticleElement procWithNextStep = CreateProcess(root, "process with Next step button", "description")
                .AddButtonsBlock()
                    .AddButton(ProcessButtonClickAction.NextStep, "Next step");

            TafEmArticleElement procWithoutButtons = CreateProcess(procWithNextStep, "process")
                .AddText();

            CreateTerminator(procWithoutButtons, "terminator with Previous step button", "terminator")
                .AddButtonsBlock()
                    .AddButton(ProcessButtonClickAction.PreviousStep, "Previous step");

            return ElementsInArticle;
        }

        //1711: Restart flow button in nested decision (level 2)
        public List<TafEmArticleElement> TestDataDF1711()
        {
            ElementsInArticle.Clear();

            TafEmArticleElement root = CreateRoot();

            TafEmArticleElement decision = CreateDecision(root, "decision", "decision");

            TafEmArticleElement yesBranch = AddBranch(decision, "yes");

            TafEmArticleElement procWithoutButtonsInBranch = CreateProcess(yesBranch, "process", "process")
                .AddText();

            TafEmArticleElement nestedDecision = CreateDecision(procWithoutButtonsInBranch, "nested decision", "decision");

            TafEmArticleElement yesBranchInNested = AddBranch(nestedDecision, "yes");

            TafEmArticleElement procWithRestartFlowButton = CreateProcess(yesBranchInNested, "process with Restart flow button", "process")
                .AddButtonsBlock()
                    .AddButton(ProcessButtonClickAction.RestartFlow, "Restart flow");

            CreateTerminator(procWithRestartFlowButton, "end of yes", "terminator");

            TafEmArticleElement noBranchInNested = AddBranch(nestedDecision, "no");

            CreateTerminator(noBranchInNested, "end of no", "terminator");

            TafEmArticleElement noBranch = AddBranch(decision, "no");

            CreateTerminator(noBranch, "end of no", "terminator");

            AddBranchesToDecisions(ElementsInArticle);

            return ElementsInArticle;
        }

        // 1712: Next/Previous step button moving to process with Next step button (flow without decisions)
        public List<TafEmArticleElement> TestDataDF1712()
        {
            ElementsInArticle.Clear();

            TafEmArticleElement root = CreateRoot();

            TafEmArticleElement procWithNextStep = CreateProcess(root, "process with Next step button", "description")
                .AddButtonsBlock()
                    .AddButton(ProcessButtonClickAction.NextStep, "Next step");

            TafEmArticleElement procWithoutButtons = CreateProcess(procWithNextStep, "process without buttons", "process")
                .AddText();

            TafEmArticleElement procWithNextPrevStep = CreateProcess(procWithoutButtons, "process with Next/Previous step button", "process")
                .AddButtonsBlock()
                    .AddButton(ProcessButtonClickAction.NextStep, "Next step")
                    .AddButton(ProcessButtonClickAction.PreviousStep, "Previous step");

            CreateTerminator(procWithNextPrevStep, "terminator", "terminator");

            return ElementsInArticle;
        }

        // 1712: Next/Previous step button moving to process with Next step button (flow without decisions)
        public List<TafEmArticleElement> TestDataDF1947()
        {
            ElementsInArticle.Clear();

            TafEmArticleElement root = CreateRoot();

            TafEmArticleElement procWithNextStep = CreateProcess(root, "proc with Next/Previous step button", "description")
                .AddButtonsBlock()
                    .AddButton(ProcessButtonClickAction.NextStep, "Next step")
                    .AddButton(ProcessButtonClickAction.PreviousStep, "Previous step");

            TafEmArticleElement procWithoutButtons = CreateProcess(procWithNextStep, "proc without buttons", "process")
                .AddText();

            CreateTerminator(procWithoutButtons, "terminator", "terminator");

            return ElementsInArticle;
        }

        //1944: Next/Previous step button in nested decision (level 2)
        public List<TafEmArticleElement> TestDataDF1944()
        {
            ElementsInArticle.Clear();

            TafEmArticleElement root = CreateRoot();

            TafEmArticleElement proc = CreateProcess(root, "proc", "process")
                .AddText();

            TafEmArticleElement decision = CreateDecision(proc, "decision L1", "decision");

            TafEmArticleElement yesBranch = AddBranch(decision, "yes");

            TafEmArticleElement procWithoutButtonsInBranch = CreateProcess(yesBranch, "proc without buttons", "process")
                .AddText();

            TafEmArticleElement nestedDecision = CreateDecision(procWithoutButtonsInBranch, "decision L2", "decision");

            TafEmArticleElement yesBranchInNested = AddBranch(nestedDecision, "yes in L2");

            TafEmArticleElement procWithoutButtonsInNested = CreateProcess(yesBranchInNested, "proc without buttons", "process");

            TafEmArticleElement procWithNextPrevButton = CreateProcess(procWithoutButtonsInNested, "process with Next/Prev step button", "process")
                .AddButtonsBlock()
                    .AddButton(ProcessButtonClickAction.NextStep, "Next step")
                    .AddButton(ProcessButtonClickAction.PreviousStep, "Previous step");

            CreateTerminator(procWithNextPrevButton, "end of yes in L2", "terminator");

            TafEmArticleElement noBranchInNested = AddBranch(nestedDecision, "no in L2");

            CreateTerminator(noBranchInNested, "end of no in L2", "terminator");

            TafEmArticleElement noBranch = AddBranch(decision, "no");

            CreateTerminator(noBranch, "end of no", "terminator");

            AddBranchesToDecisions(ElementsInArticle);

            return ElementsInArticle;
        }

        //001: Restart flow button in terminator
        public List<TafEmArticleElement> TestDataDF001()
        {
            ElementsInArticle.Clear();

            TafEmArticleElement root = CreateRoot();

            TafEmArticleElement procWithNextStep = CreateProcess(root, "process with Next step button", "description")
               .AddButtonsBlock()
                   .AddButton(ProcessButtonClickAction.NextStep, "Next step");

            TafEmArticleElement procWithoutButtons = CreateProcess(procWithNextStep, "process", "process")
                .AddText();

            CreateTerminator(procWithoutButtons, "terminator", "terminator")
                .AddButtonsBlock()
                   .AddButton(ProcessButtonClickAction.RestartFlow, "Restart flow");

            return ElementsInArticle;
        }

        //1991: DF - Step-by-step
        public List<TafEmArticleElement> TestDataDF1991()
        {
            ElementsInArticle.Clear();

            TafEmArticleElement root = CreateRoot();

            TafEmArticleElement proc = CreateProcess(root, "proc with step-by-step", "process")
                .AddStepByStepBlock()
                    .AddStep("", GetImagePath("image_01.jpg"))
                    .AddStep("", GetImagePath("image_02.jpg"))
                    .AddStep("", imageFilePath: GetImagePath("image_06.png"), "LINK", "https://www.google.com");

            CreateTerminator(proc, "terminator");

            return ElementsInArticle;
        }

        //2059: simple DF for tests of article operations (e.g. article archiving, restoring)
        public List<TafEmArticleElement> TestDataDF2059()
        {
            ElementsInArticle.Clear();

            TafEmArticleElement root = CreateRoot();

            TafEmArticleElement decision = CreateDecision(root, "Select an option", "description");

            TafEmArticleElement branch1 = AddBranch(decision, "answer 1"); //branch 1

            CreateTerminator(branch1, "End of 1");

            TafEmArticleElement branch2 = AddBranch(decision, "answer 2");

            CreateTerminator(branch2, "End of 2");

            AddBranchesToDecisions(ElementsInArticle);

            return ElementsInArticle;
        }

        // ---- Helper methods ----
        public void AddBranchesToDecisions(List<TafEmArticleElement> articleElements)
        {
            foreach (var element in articleElements)
            {
                if (element.ElementType == ArticleContentElementType.Decision)
                {
                    foreach (var id in element.ChildrenIds)
                    {
                        TafEmArticleElement branch = dataHelper.GetElementById(id, articleElements);

                        if (branch != null)
                        {
                            element.DecisionBranches.Add(branch);
                        }
                    }
                }
            }
        }

        public TafEmArticleElement CreateArticeElementAndAddToElementList(ArticleContentElementType type, int position = 0, TafEmArticleElement parent = null, string title = "", string description = "", object data = null)
        {
            TafEmArticleElement articleElement = new TafEmArticleElement()
            {
                Title = title,

                Description = description,

                ElementType = type,

                ElementPosition = position,

                Data = data,

                Id = GenerateElementId(ElementsInArticle)
            };

            ElementsInArticle.Add(articleElement);

            if (parent != null)
            {
                articleElement.SetParent(parent);
            }

            return articleElement;
        }

        public int GenerateElementId(List<TafEmArticleElement> elementsAlreadyOnTree)
        {
            int id = 0;

            if (elementsAlreadyOnTree.Count != 0)
            {
                foreach (var element in elementsAlreadyOnTree)
                {
                    if (element.Id > id)
                    {
                        id = element.Id;
                    }
                }
            }

            //int max = elementsAlreadyOnTree.Max(e => e.Id);

            return ++id;
        }

        public TafEmArticleElement CreateRoot() =>
            CreateArticeElementAndAddToElementList(ArticleContentElementType.Root, 0, null, "root");

        public TafEmArticleElement CreateProcess(TafEmArticleElement parent, string title = "", string description = "") =>
            CreateArticeElementAndAddToElementList(ArticleContentElementType.Process, 0, parent, title, description,
                new DxFlowProcessBlockData());

        public TafEmArticleElement CreatePredefinedProcess(string guideTitle, UiContentType guideType,
                                               TafEmArticleElement parent, string title = "", string description = "") =>
            CreateArticeElementAndAddToElementList(ArticleContentElementType.PredefinedProcess, 0, parent, title, description,
                new DxFlowPredefinedProcessBlockData() { GuideTitle = guideTitle, GuideType = guideType });

        public TafEmArticleElement CreateDecision(TafEmArticleElement parent, string title = "", string description = "", bool hasButtonView = true) =>
            CreateArticeElementAndAddToElementList(ArticleContentElementType.Decision, 0, parent, title, description,
                new TafEmDecisionData() { HasBranchesButtonView = hasButtonView });

        public TafEmArticleElement AddBranch(TafEmArticleElement decision, string answer, string imageFilePath="") =>
            CreateArticeElementAndAddToElementList(ArticleContentElementType.Branch,
                decision.ChildrenIds.Count + 1, decision, "", "", new TafEmBranchData() 
                {
                    Answer = answer,
                    HasImage = !string.IsNullOrEmpty(imageFilePath),
                    ImageFilePath = imageFilePath 
                });

        public TafEmArticleElement CreateTerminator(TafEmArticleElement parent, string title = "", string description = "") =>
            CreateArticeElementAndAddToElementList(ArticleContentElementType.Terminator, 0, parent, title, description,
                new DxFlowProcessBlockData());

        public TafEmArticleElement CreateInternalConnector(TafEmArticleElement pointTo, TafEmArticleElement parent = null)
        {
            TafEmArticleElement intConnector = CreateArticeElementAndAddToElementList(ArticleContentElementType.InternalConnector,
                0, parent);

            pointTo.SetParent(intConnector);

            intConnector.IntConnectorConnectionPoint = pointTo;

            return intConnector;
        }

        public void AddExternalConnector(Func<bool, List<TafEmArticleElement>> getExternalConnector, TafEmArticleElement parent, string externalFlowTitle, string title, string description="", bool isAuthoring=false)
        {
            TafEmArticleElement externalConnector = CreateArticeElementAndAddToElementList(ArticleContentElementType.ExternalConnector,
                0, parent, title, description, new DxFlowExternalConnectorBlockData() { ExternalFlowTitle = externalFlowTitle});

            if (!isAuthoring)
            {
                getExternalConnector.Invoke(true);

                int rootToRemovePosition = ElementsInArticle.FindIndex(e => e.ElementType == ArticleContentElementType.Root && e.Id > 1);

                if (rootToRemovePosition > 0)
                {
                    ElementsInArticle[rootToRemovePosition + 1].SetParent(externalConnector);

                    ElementsInArticle.RemoveAt(rootToRemovePosition);
                }
            }
        }

        public string GetImagePath(string imageFileName) => 
            Path.GetFullPath(
                Path.Combine(CommonHelper.GetTestSolutionPath(), SecretsHelper.ReadSecretValue(testConfig.ConfigRoot, "TestImagesFolder"), imageFileName)
                );
    }
}
