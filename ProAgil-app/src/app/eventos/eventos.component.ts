import { Component, OnInit, TemplateRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { Evento } from '../_models/Evento';
import { EventoService } from '../_services/evento.service';
import {defineLocale} from 'ngx-bootstrap/chronos';
import {BsLocaleService} from 'ngx-bootstrap/datepicker';
import { ToastrService } from 'ngx-toastr';

import {ptBrLocale} from 'ngx-bootstrap/locale';
import { templateJitUrl } from '@angular/compiler';
defineLocale('pt-br', ptBrLocale);

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.css']
})
export class EventosComponent implements OnInit {
  eventosFiltrados: Evento [];
  eventos: Evento [];
  evento: Evento;
  bodyDeletarEvento = '';
  modoSalvar = 'post';
  imagemLargura = 50;
  imagemMargem = 2;
  mostrarImagem = false;
  modalRef: BsModalRef;
  registerForm: FormGroup;
  titulo = 'Eventos';
  file: File;
  _filtroLista = '';

  constructor(
    private eventoService: EventoService,
    private modalService: BsModalService,
    private fb: FormBuilder,
    private localeService: BsLocaleService,
    private toastr: ToastrService
    ) {
      this.localeService.use('pt-br');
    }

  get filtroLista(): string{
    return this._filtroLista;
  }
  set filtroLista(value: string){
    this._filtroLista = value;
    this.eventosFiltrados = this.filtroLista ? this.filtrarEventos(this.filtroLista) : this.eventos;
  }

  editarEvento(template: any, idEvento: number): void{
    this.modoSalvar = 'put';
    this.eventoService.getEventoById(idEvento).subscribe(
      (_evento: Evento) => {
        this.openModal(template);
        _evento.imagemURL = '';
        this.registerForm.patchValue(_evento);
        this.evento = _evento;
      }, error => {
        console.log(error);
      }
    );
  }

  novoEvento(template: any): void{
    this.modoSalvar = 'post';
    this.openModal(template);
  }

  openModal(template: any): void{
    this.registerForm.reset();
    template.show(template);
  }

  ngOnInit(): any{
    this.validation();
    this.getEventos();
  }

  filtrarEventos(filtrarPor: string): Evento[]{
    filtrarPor = filtrarPor.toLocaleLowerCase();
    return this.eventos.filter(
      evento => evento.tema.toLocaleLowerCase().indexOf(filtrarPor) !== -1
    );
  }

  alternarImagem(): void{
    this.mostrarImagem = !this.mostrarImagem;
  }

  validation(): void{
    this.registerForm = this.fb.group({
      tema: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]],
      local: ['', Validators.required],
      dataEvento: ['', Validators.required],
      qtdPessoas: ['', [Validators.required, Validators.max(120000)]],
      imagemURL: ['', Validators.required],
      telefone: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]]
    });
  }

  excluirEvento(evento: Evento, template: any): void{
    this.openModal(template);
    this.evento = evento;
    this.bodyDeletarEvento = `Tem certeza que deseja excluir o Evento: ${evento.tema}, Código: ${evento.id}`;
  }

  confirmeDelete(template: any): void{
    this.eventoService.deleteEvento(this.evento.id).subscribe(
      () => {
          template.hide();
          this.getEventos();
          this.toastr.success('Registro deletado com sucesso!');
        }, error => {
          this.toastr.error(`Erro ao deletar registro: ${error}`);
          console.log(error);
        }
    );
  }

  upLoadImage(){    
    const nomeArquivo = this.evento.imagemURL.split('\\', 3);
    this.evento.imagemURL = nomeArquivo[2];

    this.eventoService.postUpload(this.file, nomeArquivo[2]).subscribe();
  }

  salvarAlteracao(template: any): void{
    if (this.registerForm.valid){
      if (this.modoSalvar === 'post'){
        this.evento = Object.assign({}, this.registerForm.value);
        this.upLoadImage();

        this.eventoService.postEvento(this.evento).subscribe(
          (novoEvento: Evento) => {
            console.log(novoEvento);
            template.hide();
            this.getEventos();
            this.toastr.success('Registro inserido com sucesso!');
          }, error => {
            this.toastr.error(`Erro ao inserir registro: ${error}`);
            console.log(error);
          }
        );
      }
      else{
        this.evento = Object.assign({id: this.evento.id}, this.registerForm.value);
        this.upLoadImage();

        this.eventoService.putEvento(this.evento, this.evento.id).subscribe(
          (novoEvento: Evento) => {
            console.log(novoEvento);
            template.hide();
            this.getEventos();
            this.toastr.success('Registro editado com sucesso!');
          }, error => {
            this.toastr.error(`Erro ao editar registro: ${error}`);
            console.log(error);
          }
        );
      }
    }
  }

  onFileChange(event): void{
    const reader = new FileReader();
    if (event.target.files && event.target.files.length){
      this.file = event.target.files;
      console.log(this.file);
    }
  }

  getEventos(): void{
    this.eventoService.getAllEvento().subscribe(
      (_eventos: Evento[]) => {
        this.eventos = _eventos;
        this.eventosFiltrados = this.eventos;
        console.log(_eventos);
      },
      error => {
        this.toastr.error(`Erro ao carregar os registros: ${error}`);
        console.log(error);
      }
    );
  }

}
