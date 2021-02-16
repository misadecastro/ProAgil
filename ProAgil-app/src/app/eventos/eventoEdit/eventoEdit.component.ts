import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { ToastrService } from 'ngx-toastr';
import { Evento } from 'src/app/_models/Evento';
import { EventoService } from 'src/app/_services/evento.service';
import {ptBrLocale} from 'ngx-bootstrap/locale';
import {defineLocale} from 'ngx-bootstrap/chronos';
import { ActivatedRoute } from '@angular/router';
defineLocale('pt-br', ptBrLocale);

@Component({
  selector: 'app-eventoEdit',
  templateUrl: './eventoEdit.component.html',
  styleUrls: ['./eventoEdit.component.scss']
})
export class EventoEditComponent implements OnInit {

  titulo = 'Editar Eventos';
  fileNameToUpdate: string;
  registerForm: FormGroup;
  evento: Evento = new Evento();
  imagemURL = 'assets/img/upload.png';
  dataAtual = '';
  file: File;

  get lotes(): FormArray{
    return <FormArray>this.registerForm.get('lotes');
  }

  get redesSociais(): FormArray{
    return <FormArray>this.registerForm.get('redesSociais');
  }

  constructor(
    private eventoService: EventoService,
    private fb: FormBuilder,
    private localeService: BsLocaleService,
    private toastr: ToastrService,
    private router: ActivatedRoute
    ) {
      this.localeService.use('pt-br');
    }

  ngOnInit() {
    this.validation();    
    this.carregarEvento();
  }

  carregarEvento(){
    const idEvento = +this.router.snapshot.paramMap.get('id');
    this.eventoService.getEventoById(idEvento).subscribe(
      (evento: Evento) => {
        this.evento = Object.assign({}, evento);
        this.fileNameToUpdate = evento.imagemURL.toString();
        this.imagemURL = `http://localhost:5000/Resources/Images/${evento.imagemURL}?_ts=${this.dataAtual}`
        this.evento.imagemURL = '';
        this.registerForm.patchValue(this.evento);
        this.evento.lotes.forEach(lote => {
          this.lotes.push(this.CriaLote(lote));
        });
        this.evento.redesSociais.forEach(redeSocial => {
          this.redesSociais.push(this.CriaRedeSocial(redeSocial));
        });
      }
    );
  }

  onFileChange(event){
    const reader = new FileReader();    
    reader.onload = (event: any) => {
      this.imagemURL = event.target.result;      
    }
    this.file = event.target.files;
    reader.readAsDataURL(event.target.files[0]);
  }
    
  CriaLote(lote: any): FormGroup{
    console.log(lote);
    return this.fb.group({
      id: [lote.id],
      nome: [lote.nome, Validators.required],
      quantidade: [lote.quantidade, Validators.required],
      preco: [lote.preco, Validators.required],
      dataInicio: [lote.dataInicio],
      dataFim: [lote.dataFim]
    });
  }

  CriaRedeSocial(redeSocial: any): FormGroup{
    console.log(redeSocial);
    return this.fb.group({
      id: [redeSocial.id],
      nome: [redeSocial.nome, Validators.required],
      url: [redeSocial.url, Validators.required],
    });
  }

  adicionarLote(){
    this.lotes.push(this.CriaLote({id:0}));
  }

  adicionarRedeSocial(){
    this.redesSociais.push(this.CriaRedeSocial({id:0}));
  }
  
  removerLote(id: number){
    this.lotes.removeAt(id);
  }

  removerRedeSocial(id: number){
    this.redesSociais.removeAt(id);
  }

  upLoadImage(){    
    if(this.registerForm.get('imagemURL').value != ''){
      console.log(this.file);
      console.log(this.fileNameToUpdate);
      this.eventoService.postUpload(this.file, this.fileNameToUpdate)
      .subscribe(
        ()=>{
          this.dataAtual = new Date().getMilliseconds().toString();
          this.imagemURL = `http://localhost:5000/Resources/Images/${this.evento.imagemURL}?_ts=${this.dataAtual}`;
        }
      );
    }
    
  }

  salvarEvento(){
    this.evento = Object.assign({id: this.evento.id}, this.registerForm.value);
    this.evento.imagemURL = this.fileNameToUpdate;
    this.upLoadImage();

    this.eventoService.putEvento(this.evento, this.evento.id).subscribe(
      (novoEvento: Evento) => {
        console.log(novoEvento);
        this.toastr.success('Registro editado com sucesso!');
      }, error => {
        this.toastr.error(`Erro ao editar registro: ${error}`);
        console.log(error);
      }
    );
  }

  validation(): void{
    this.registerForm = this.fb.group({
      id: [],
      tema: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]],
      local: ['', Validators.required],
      dataEvento: ['', Validators.required],
      qtdPessoas: ['', [Validators.required, Validators.max(120000)]],
      imagemURL: [''],
      telefone: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      lotes: this.fb.array([]),
      redesSociais: this.fb.array([]),
    });
  }

}
