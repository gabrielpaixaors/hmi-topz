# HMI Topz

**Ferramenta de Otimização de Arquivos de Sinótico para IHM**  
Solução desktop (WPF/.NET Framework) que automatiza o pós-processamento de arquivos de sinótico de IHM, removendo parâmetros desnecessários e reformando o layout para melhorar desempenho.

## Tecnologias
- C#  
- WPF (.NET Framework 4.7.2)  
- MVVM Light  
- Leitura e escrita de XML  
- **Expressões Regulares (Regex)** para identificar padrões e filtrar parâmetros de forma flexível

## Funcionalidades
1. **Importação** de arquivos de sinótico (XML).  
2. **Filtragem automática** de parâmetros não utilizados, combinando regras configuráveis e Regex.  
3. **Reformatação** do XML para manter apenas tags essenciais e otimizar tamanho.  
4. **Exportação** do sinótico otimizado para recarga na IHM.  
5. **Logs** detalhados de remoções e tamanhos antes/depois.

## Benefícios
- Substituiu um fluxo manual que levava horas por um processo de segundos.  
- Padronização e redução de erros de sintaxe em campo.

## Como rodar
1. Clone o repositório e abra `HMITopz.sln` no Visual Studio 2019+.  
2. Restaure pacotes NuGet.  
3. Compile e execute.  
4. Selecione o arquivo `.xml/mimic` de sinótico e clique em .
5. Selecione local novo arquivo `.xml/mimic` de sinótico e clique em “Processar”.
