using RTLTMPro;
using Sirenix.OdinInspector;
using UnityEngine;

namespace QuizSystem
{
    public class Calculator : MonoBehaviour
    {
        public RTLTextMeshPro dispalyText;
        private string currentInput = "";
        private double result = 0.0;
        private string CurrentCalucalution = "";

        public double RESULT { get { return result; } }


        private void OnEnable()
        {
            ClearInput();
        }
        public void OnButtonClick(string buttonValue)
        {
            if (buttonValue == "=")
            {
                CalculateResult();
            }
            else if (buttonValue == "C")
            {
                ClearInput();
            }
            else if (buttonValue == "(" || buttonValue == ")")
            {
                currentInput += buttonValue;
                UpdateDisplay();
            }
            else if (buttonValue == "CE")
            {
                Backspace();
            }
            else
            {
                Debug.Log("Button: " + buttonValue + "  IS CLICKED");
                currentInput += buttonValue;
                UpdateDisplay();
            }


        }

        public void CalculateResult()
        {
            try
            {
                CurrentCalucalution = currentInput;
                result = System.Convert.ToDouble(new System.Data.DataTable().Compute(currentInput, ""));
                Debug.Log("result: " + result);
                currentInput = result.ToString();

                UpdateDisplay();

            }
            catch (System.Exception)
            {
                currentInput = string.Empty;
                UpdateDisplay();
            }

        }

        public void ClearInput()
        {
            currentInput = string.Empty;
            result = 0.0;
            UpdateDisplay();
        }

        public void Backspace()
        {
            if (currentInput.Length > 0)
            {
                currentInput = currentInput.Substring(0, currentInput.Length - 1);
                UpdateDisplay();
            }
        }

        public void UpdateDisplay()
        {

            dispalyText.text = currentInput;
            Debug.Log("Update value:  " + currentInput + " dispalyText: " + dispalyText.text);
        }

        [Button("GetCurrentText")]
        public string CurrentCalucalutionText()
        {
            return CurrentCalucalution;
        }
    }
}

