using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    /// <summary>
    /// ������ ��� ������� �����
    /// </summary>
    public Button StartButton;
    /// <summary>
    /// ���� ����� ���������� ������
    /// </summary>
    public InputField CountCell;
    /// <summary>
    ///  ���� ����� ���������� ������������ ������
    /// </summary>
    public InputField CountAntivirus;
    /// <summary>
    /// ����� ���������
    /// </summary>
    public Text Validation;

    void Start()
    {
        //��������� ������� ������ ����� � ���� ����� 
        CountCell.characterValidation = InputField.CharacterValidation.Integer;
        CountAntivirus.characterValidation = InputField.CharacterValidation.Integer;
        //��������� ���������
        Validation.enabled = false;
    }

    public void OnClickStart()
    {
        int countCells;
        int countAntivirus;

        //�������� ��� ���� ���������� ������ �� ������
        if (string.IsNullOrWhiteSpace(CountCell.text))
        {
            Validation.enabled = true;
            return;
        }

        //������� �������� ����� �����
        if(int.TryParse(CountCell.text, out countCells))
        {
            //���� 0 �� �� ������ ��������� �����
            if(countCells == 0)
            {
                Validation.enabled = true;
                return;
            }

            Validation.enabled = false;

            //������� �������� ���������� ������������ ������
            if(int.TryParse(CountAntivirus.text, out countAntivirus))
            {
                Data.StartAntivirus = countAntivirus;
            }

            //�������� ��������� � ���������� ����� ��� �����
            Data.StartCells = countCells;
            //������ �����
            SceneManager.LoadScene("Level 1");
        }
    }
}
