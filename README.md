# Sistemas Computacionais em Tempo Real - Projeto Final
Módulo 2 - Identificador de Curto-Circuito IEC com Funções de Proteção 50/51

Este projeto visa desenvolver um sistema computacional em tempo real para **identificar curtos-circuitos de acordo com a norma IEC**, implementando funções de proteção 50/51 da tabela ANSI.

## Objetivo

O objetivo principal deste projeto é criar um sistema capaz de monitorar a corrente elétrica em tempo real, identificar variações que possam indicar a ocorrência de um curto-circuito e iniciar medidas de proteção adequadas para evitar danos ao sistema elétrico.

## Funcionalidades Principais

- **Monitoramento de Corrente**: o sistema irá monitorar continuamente a corrente elétrica - isso será feito a partir de pacotes enviados por um gerador de pacotes simulando a rede elétrica. Se houver um aumento repentino na corrente, o sistema iniciará um temporizador para verificar se a corrente permanece alta por um período de tempo significativo. Para isso, o sistema seguirá a curva de curto-circuito especificada pela norma IEC para garantir que as medidas de proteção sejam acionadas no tempo correto.

- **Identificação de Eventos de Curto-Circuito**: caso o sistema detecte um evento de curto-circuito, ele enviará um pacote especial imediatamente para o módulo responsável (acionador).

- **Identificação de Fim de Evento**: caso a corrente normalize, o sistema enviará um pacote identificando o fim do evento.

## Conteúdo do Trabalho

Os módulos do projeto são:
- **Módulo 1 - Gerador de Pacotes**: Sistema responsável por simular até 5 dispositivos geradores de dados de corrente.
- **Módulo 2 - Identificador de Curto (com visual)**: Sistema responsável por analisar os dados e identificar eventos elétricos na rede, com monitoramento gráfico da última corrente recebida. 
- **Módulo 2 - Identificador de Curto (sem visual)**: Semelhante ao anterior, mas sem monitoramento gráfico da última corrente recebida.
- **Módulo Extra - Receptor de Pacotes para Teste**: Aplicação de console para receber pacotes enviados no alarme do módulo 2, com intenção de ser apenas ilustrativo.

## Utilização do Programa

Para que o programa seja utilizado corretamente, segue algumas instruções:
1) Abra todos os módulos (no caso do módulo identificador, escolhe-se entre o com visuais ou sem visuais) e os inicia;
2) Acione o botão iniciar primeiro no módulo identificador;
3) Adicione até 5 dispositivos geradores de pacotes e acione o botão inicie para que a comunicação seja estabelecida;
4) Após isso, os testes ajustando as correntes podem ser feitos e os pacotes de alarme (quando necessários) serão enviados ao módulo de teste para visualização dos pacotes.  
