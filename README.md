# Sistemas Computacionais em Tempo Real - Projeto Final
Módulo 2 - Identificador de Curto-Circuito IEC com Funções de Proteção 50/51

Este projeto visa desenvolver um sistema computacional em tempo real para identificar curtos-circuitos de acordo com a norma IEC, implementando funções de proteção 50/51 da tabela ANSI.

## Objetivo

O objetivo principal deste projeto é criar um sistema capaz de monitorar a corrente elétrica em tempo real, identificar variações que possam indicar a ocorrência de um curto-circuito e iniciar medidas de proteção adequadas para evitar danos ao sistema elétrico.

## Funcionalidades Principais

- Monitoramento de Corrente: o sistema irá monitorar continuamente a corrente elétrica - isso será feito a partir de pacotes enviador por um gerador de pacotes simulando a rede elétrica. Se houver um aumento repentino na corrente, o sistema iniciará um temporizador para verificar se a corrente permanece alta por um período de tempo significativo. Para isso, o sistema seguirá a curva de curto-circuito especificada pela norma IEC para garantir que as medidas de proteção sejam acionadas no tempo correto.

- Identificação de Eventos de Curto-Circuito: caso o sistema detecte um evento de curto-circuito, ele enviará um pacote especial imediatamente para o módulo responsável (acionador).

- Comunicação na Rede: em caso de detecção de um curto-circuito, o sistema enviará um pacote de dados pela rede para alertar outros dispositivos conectados sobre a ocorrência do evento.
