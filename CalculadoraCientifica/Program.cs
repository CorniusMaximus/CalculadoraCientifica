// See https://aka.ms/new-console-template for more information
using System.Globalization;
using System.Text.RegularExpressions;

// Definição de padrões
string _patternBasic = @"\d+[,.]?\d*\s*[+\-*/]\s*\d+[,.]?\d*";
string[] _patternsOps = [@"\d+[,.]?\d*\s*[\*\/]\s*\d+[,.]?\d*", @"\d+[,.]?\d*\s*[\+\-]\s*\d+[,.]?\d*"];
string _patternNumber = @"\d+[,.]?\d*";
string _patternSymbol = @"[+\-*/]";
string _patternParenthesis = @"\([^()]*\)";
string _patternSimpleParenthesis = @"\(\s*\-*\d+[,.]?\d*\s*\)";
string _patternNegateSum = @"(\+\s*\-|\-\s*\+)";

// Input da expressão
Console.WriteLine("Insira uma expressão matemática");
string _expression = Console.ReadLine();

// Checagem da expressão
while(!Regex.IsMatch(_expression, _patternBasic))
{
    Console.WriteLine("Insira uma expressão válida");
    _expression = Console.ReadLine();
}

// Início do loop de cálculo
while(Regex.IsMatch(_expression, _patternBasic))
{
    // Extrai o parêntesis mais interno
    string _extractedParenthesis;
    if(Regex.IsMatch(_expression, _patternParenthesis))
        _extractedParenthesis = Regex.Match(_expression, _patternParenthesis).Value;
    else
        _extractedParenthesis = _expression;

    // Extrai a equação mais interna de acordo com a regra de importância
    string _extractedExpression = _extractedParenthesis;
    foreach(var _pattern in _patternsOps)
    {
        if(Regex.IsMatch(_expression, _pattern))
        {
            _extractedExpression = Regex.Match(_expression, _pattern).Value;
            break;
        }
    }

    // Extrai os valores separadamente
    MatchCollection _numbers = Regex.Matches(_extractedExpression, _patternNumber);
    float _num1 = float.Parse(_numbers[0].Value);
    float _num2 = float.Parse(_numbers[1].Value);
    string _operation = Regex.Match(_extractedExpression, _patternSymbol).Value;

    // Realiza as opeações básicas
    float _result = 0;
    switch(_operation)
    {
        case "+":
            _result = _num1 + _num2;
            break;

        case "-":
            _result = _num1 - _num2;
            break;

        case "*":
            _result = _num1 * _num2;
            break;

        case "/":
            _result = _num1 / _num2;
            break;

        default:
            break;
    }

    // Substitui na expressão original o novo valor calculado
    string _newParenthesis = _extractedParenthesis.Replace(oldValue: _extractedExpression, newValue: _result.ToString());

    if(Regex.IsMatch(_newParenthesis, _patternSimpleParenthesis))
        _newParenthesis = Regex.Replace(_newParenthesis, @"[\(\)]", "");

    _expression = _expression.Replace(oldValue: _extractedParenthesis, newValue: _newParenthesis);

    if(Regex.IsMatch(_expression, _patternNegateSum))
        _expression = Regex.Replace(_expression, _patternNegateSum, "-");
}

Console.WriteLine(_expression);

Console.ReadKey();
