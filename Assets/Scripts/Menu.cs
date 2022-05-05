using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    /// <summary>
    /// Кнопка для запуска сцены
    /// </summary>
    public Button StartButton;
    /// <summary>
    /// Поле ввода количество клеток
    /// </summary>
    public InputField CountCell;
    /// <summary>
    ///  Поле ввода количество антивирусных клеток
    /// </summary>
    public InputField CountAntivirus;
    /// <summary>
    /// Текст валидации
    /// </summary>
    public Text Validation;

    void Start()
    {
        //Позволяет вводить только цифры в поля ввода 
        CountCell.characterValidation = InputField.CharacterValidation.Integer;
        CountAntivirus.characterValidation = InputField.CharacterValidation.Integer;
        //Валидация отключена
        Validation.enabled = false;
    }

    public void OnClickStart()
    {
        int countCells;
        int countAntivirus;

        //Проверка что поле количество клеток не пустое
        if (string.IsNullOrWhiteSpace(CountCell.text))
        {
            Validation.enabled = true;
            return;
        }

        //Попытка получить целое число
        if(int.TryParse(CountCell.text, out countCells))
        {
            //Если 0 то не давать запускать сцену
            if(countCells == 0)
            {
                Validation.enabled = true;
                return;
            }

            Validation.enabled = false;

            //Попытка получить количество антивирусных клеток
            if(int.TryParse(CountAntivirus.text, out countAntivirus))
            {
                Data.StartAntivirus = countAntivirus;
            }

            //Передача аргумента в глобальный класс для сцены
            Data.StartCells = countCells;
            //Запуск сцены
            SceneManager.LoadScene("Level 1");
        }
    }
}
